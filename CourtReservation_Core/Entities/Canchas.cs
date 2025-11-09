using System;
using System.Collections.Generic;

namespace CourtReservation_Core.Entities;

public partial class Canchas : BaseEntity
{

    public string Nombre { get; set; } = null!;

    public string TipoDeporte { get; set; } = null!;

    public decimal PrecioPorHora { get; set; }

    public string? Ubicacion { get; set; }

    public bool EstaActiva { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<HorariosDisponibles> HorariosDisponibles { get; set; } = new List<HorariosDisponibles>();

    public virtual ICollection<Reservas> Reservas { get; set; } = new List<Reservas>();
}
