using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportingSolution.Data.NoSqlModel;

namespace ReportingSolution.Data.Configuration
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>, INoSqlEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.Property(x => x.ProductName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
