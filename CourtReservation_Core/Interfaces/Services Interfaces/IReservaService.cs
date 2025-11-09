using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.QueryFilters;

namespace CourtReservation_Core.Interfaces.Services_Interfaces
{
    public interface IReservaService
    {
        Task<ResponseData> GetAllReservasAsync(ReservaQueryFilter filters);
        Task<Reservas> GetReservaAsync(int id);
        Task<IEnumerable<ReservaQueryFilter>> GetReservas24Async();
        Task<IEnumerable<NroReservasPorUsuario>> GetNroReservasPorUsuarioAsync();
        Task<IEnumerable<ReservasConPagoRealizado>> GetReservasConPagoRealizadoAsync();

        Task InsertReservaAsync(Reservas reserva);
        Task UpdateReservaAsync(Reservas reserva);
        Task DeleteReservaAsync(Reservas reserva);
    }
}
