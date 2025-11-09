using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.Entities;

namespace CourtReservation_Core.CustomEntities
{
    public class NroReservasPorUsuario
    {

        public int UsuarioId { get; set; }
        public int ReservationCount { get; set; }
    }
}
