using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.QueryFilters;

namespace CourtReservation_Core.Interfaces
{
    public interface IReservaRepository : IBaseRepository<Reservas>
    {
        Task<IEnumerable<ReservaQueryFilter>> GetAllReservas24DapperAsync(int limit = 10);
        Task<IEnumerable<NroReservasPorUsuario>> GetNroReservasPorUsuarioAsync();
        Task<IEnumerable<ReservasConPagoRealizado>> GetReservasConPagoRealizadoAsync();
    }
}
