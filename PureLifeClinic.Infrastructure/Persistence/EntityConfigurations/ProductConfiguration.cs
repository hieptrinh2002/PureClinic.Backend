using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Persistence.EntityConfigurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //builder.Property<uint>("Version").IsRowVersion();    

            builder.HasQueryFilter(p => !p.IsDeleted && p.IsActive);
            // to ignore query filter, user .IgnoreQueryFilters() method    
        }
    }
}
