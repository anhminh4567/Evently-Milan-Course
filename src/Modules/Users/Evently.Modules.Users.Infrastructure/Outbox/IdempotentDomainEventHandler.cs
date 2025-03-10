using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Common.Infrastructure.Outbox;

namespace Evently.Modules.Users.Infrastructure.Outbox;


// This is a decoreated class for the DOmainEventHandler<T> 
// we do this in dependency injection
internal sealed class IdempotentDomainEventHandler<TDomainEvent>    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly IDomainEventHandler<TDomainEvent> decorated;
    private readonly IDbConnectionFactory dbConnectionFactory;

    public IdempotentDomainEventHandler(IDomainEventHandler<TDomainEvent> decorated, IDbConnectionFactory dbConnectionFactory)
    {
        this.decorated = decorated;
        this.dbConnectionFactory = dbConnectionFactory;
    }

    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var outboxMessageConsumer = new OutboxMessageConsumer(Guid.Parse(domainEvent.Id), decorated.GetType().Name);
        // we check here IF we have consume this message yet ??? ( from OutboxMessageConsumer )
        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);
        // if we consumer success => insert into table "OutboxMessageConsumer"
        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }

    private static async Task<bool> OutboxConsumerExistsAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql = 
            """
            SELECT EXISTS(
                SELECT 1
                FROM users."OutboxMessageConsumers"
                WHERE "OutboxMessageId" = @OutboxMessageId AND
                      "Name" = @Name
            )
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO users."OutboxMessageConsumers"("OutboxMessageId", "Name")
            VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
