using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data.Configuration
{
    internal sealed class SportConfiguration : IEntityTypeConfiguration<Sport>, INoSqlEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Sport> modelBuilder)
        {
        }
    }
}
