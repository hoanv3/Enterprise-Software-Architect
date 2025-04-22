using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data.Configuration
{
    public class BankingEventDataConfiguration : IEntityTypeConfiguration<BankingEventData>, INoSqlEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<BankingEventData> builder)
        {
            builder.Property(x => x.EventType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Data)
                .HasMaxLength(10000)
                .IsRequired();
        }
    }
}
