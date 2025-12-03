using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourtReservation_Infraestructure.EntitiesConfigurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reservas>
    {
        public void Configure(EntityTypeBuilder<Reservas> builder)
        {
            builder.ToTable("Reservas");
            builder.HasKey(e => e.Id).HasName("PK__Reservas__3214EC077C1ECB06");

        builder.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
        builder.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        builder.Property(e => e.FechaFin).HasColumnType("datetime");
        builder.Property(e => e.FechaInicio).HasColumnType("datetime");
        builder.Property(e => e.Notas).HasMaxLength(300);

        builder.HasOne(d => d.Cancha).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.CanchaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Canchas");

        builder.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Usuarios");
        }

    }
}
