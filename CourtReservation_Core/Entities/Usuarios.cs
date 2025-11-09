using System;
using System.Collections.Generic;

namespace CourtReservation_Core.Entities;

public partial class Usuarios : BaseEntity
{

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Reservas> Reservas { get; set; } = new List<Reservas>();
}
