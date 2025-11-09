using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtReservation_Core.CustomEntities
{
    public class ReservasConPagoRealizado
    {

        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Notas { get; set; }
        public string Estado { get; set; } 
        public string NombreUsuario { get; set; } 
        public string NombreCancha { get; set; }
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; }
    }
}
