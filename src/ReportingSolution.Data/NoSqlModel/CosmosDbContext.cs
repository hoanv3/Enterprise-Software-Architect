using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using ReportingSolution.Data.Configuration;

namespace ReportingSolution.Data.NoSqlModel
{
    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseDocument>()
                .HasDiscriminator<string>("type")
                .HasValue<Sport>(nameof(Sport))
                .HasValue<Product>(nameof(Product))
                .HasValue<SalesInfo>(nameof(SalesInfo))
                .HasValue<BankingEventData>(nameof(BankingEventData));
            modelBuilder
                .Entity<BaseDocument>()
                .ToContainer("Sports")
                .HasPartitionKey(x => x.Section)
                .HasKey(x => x.Id);
            modelBuilder
                .Entity<BaseDocument>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<BaseDocument>()
                .Property(x => x.Etag)
                .IsETagConcurrency();

            var currentAssembly = typeof(CosmosDbContext).Assembly;
            var typesToRegister = currentAssembly
                .GetTypes()
                .Where(type =>
                !type.IsAbstract &&
                !type.IsGenericTypeDefinition &&
                typeof(INoSqlEntityTypeConfiguration).IsAssignableFrom(type))
                .ToList();

            foreach (var type in typesToRegister)
            {
                // Find the IEntityTypeConfiguration<T> interface
                var @interface = type
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

                if (@interface is not null)
                {
                    var configuredInstance = Activator.CreateInstance(type);
                    Guard.Against.Null(configuredInstance, nameof(configuredInstance));
                    modelBuilder.ApplyConfiguration((dynamic)configuredInstance);
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
