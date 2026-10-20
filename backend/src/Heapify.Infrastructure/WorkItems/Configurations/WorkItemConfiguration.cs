using Heapify.Domain.Projects.Entities;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Heapify.Infrastructure.WorkItems.Configurations;

internal sealed class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.HasKey(w => new { w.ProjectId, w.Number });

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(w => w.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}