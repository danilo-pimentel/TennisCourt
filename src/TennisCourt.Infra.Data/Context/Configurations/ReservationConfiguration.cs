using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.Data.Context.Configurations.Base;

namespace TennisCourt.Infra.Data.Context.Configurations
{
    public class ReservationConfiguration : BaseEntityTypeConfiguration<Reservation>
    {

        public override void Configure(EntityTypeBuilder<Reservation> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.RefundValue);

            builder.Property(x => x.ReservationStatus);

            builder.Property(x => x.Value);

            builder.Property(x => x.Date);

            builder.Property(x => x.Name);

            builder.Property(x => x.Phone);
        }
    }
}
