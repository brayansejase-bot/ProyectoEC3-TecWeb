using System;
using System.Collections.Generic;

namespace CourtReservation_Core.Entities;

public partial class Pagos : BaseEntity
{

    public int ReservaId { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string EstadoPago { get; set; } = null!;

    public DateTime? FechaPago { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual Reservas Reserva { get; set; } = null!;
}
