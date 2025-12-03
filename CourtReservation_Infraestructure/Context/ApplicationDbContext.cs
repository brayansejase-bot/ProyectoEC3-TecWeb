using System;
using System.Collections.Generic;
using System.Reflection;
using CourtReservation_Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourtReservation_Infraestructure.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Canchas> Canchas { get; set; }

    public virtual DbSet<HorariosDisponibles> HorariosDisponibles { get; set; }

    public virtual DbSet<Pagos> Pagos { get; set; }

    public virtual DbSet<Reservas> Reservas { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Security> Security { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
