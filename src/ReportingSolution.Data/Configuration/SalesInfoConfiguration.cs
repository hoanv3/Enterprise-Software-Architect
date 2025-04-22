using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data.Configuration
{
    internal sealed class SalesInfoConfiguration : IEntityTypeConfiguration<SalesInfo>, INoSqlEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<SalesInfo> modelBuilder)
        {
            modelBuilder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);
        }
    }
}
