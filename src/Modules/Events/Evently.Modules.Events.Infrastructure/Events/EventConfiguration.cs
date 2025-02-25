using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
        builder.Navigation(e => e.Category );
    }
}
