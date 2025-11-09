using System;
using System.Collections.Generic;

namespace CourtReservation_Core.Entities;

public partial class HorariosDisponibles : BaseEntity
{

    public int CanchaId { get; set; }

    public int DiaSemana { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Canchas Cancha { get; set; } = null!;
}
