using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Bogus;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Evently.Modules.Users.Infrastructure.Identity.KeyCloakAuthDelegatingHandler;

namespace Evently.Modules.Users.IntegrationTests.Abstractions;

// this is to said that this test belong tot he same Collection Fixture we defind in the same foldeer
//[Collection(nameof(IntegrationTestCollection))]
[Collection("IntegrationTestCollection")]
public class BaseIntegrationTest : IDisposable
{
    //IntegrationTestWebAppFactory _factory;
    private readonly IServiceScope _scope;
    protected readonly ISender _sender;
    protected static readonly Faker Faker = new Faker();
    protected readonly HttpClient HttpClient;
    private readonly KeyCloakOptions _options;
    protected readonly UsersDbContext DbContext;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
       _scope = factory.Services.CreateScope();
        _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        HttpClient = factory.CreateClient();
        _options = _scope.ServiceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
        DbContext = _scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    }
    protected async Task<string> GetAccessTokenAsync(string email, string password)
    {
        using var client = new HttpClient();

        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.PublicClientId),
            new("scope", "openid"),
            new("grant_type", "password"),
            new("username", email),
            new("password", password)
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));
        authRequest.Content = authRequestContent;

        using HttpResponseMessage authorizationResponse = await client.SendAsync(authRequest);

        authorizationResponse.EnsureSuccessStatusCode();

        AuthToken authToken = await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>();

        return authToken!.AccessToken;
    }
    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }
    public void Dispose()
    {
        _scope.Dispose();
    }
}
