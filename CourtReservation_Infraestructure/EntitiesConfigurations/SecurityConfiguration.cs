using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourtReservation_Infraestructure.EntitiesConfigurations
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder) 
        {
                builder.ToTable("Security");

                builder.Property(e => e.Login)
                 .HasMaxLength(50)
                 .IsUnicode(false)
                 .HasColumnName("login");
                 builder.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                builder.Property(e => e.Password)
                        .HasMaxLength(200)
                        .IsUnicode(false);
            builder.Property(e => e.Role)
                    .HasMaxLength(15) 
                    .IsUnicode(false)
                    .HasConversion(
                         x => x.ToString(),
                         x => (RoleType)Enum.Parse(typeof(RoleType), x)
                    );


        }
    }
}
