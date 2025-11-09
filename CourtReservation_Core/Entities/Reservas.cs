using System;
using System.Collections.Generic;

namespace CourtReservation_Core.Entities;

public partial class Reservas : BaseEntity
{

    public int UsuarioId { get; set; }

    public int CanchaId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public string? Notas { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Canchas Cancha { get; set; } = null!;

    public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
