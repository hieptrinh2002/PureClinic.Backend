using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.EntityConfigurations
{
    internal class AppoimentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
        }
    }
}
