using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Exceptions;
using CourtReservation_Core.Interfaces;
using CourtReservation_Core.Interfaces.Services_Interfaces;
using CourtReservation_Core.QueryFilters;

namespace CourtReservation_Core.Services
{
    public class ReservaService : IReservaService
    {
        //public readonly IBaseRepository<Reservas> _reservaRepository;
        //public readonly IBaseRepository<Usuarios> _usuarioRepository;
        //public readonly IBaseRepository<Canchas> _canchaRepository;
        public readonly IUnitOfWork _unitOfWork;
        public ReservaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseData> GetAllReservasAsync(ReservaQueryFilter filters)
        {
            var reservas = await _unitOfWork.ReservaRepository.GetAll();

            if (filters.userId != null)
            {
                reservas = reservas.Where(x => x.UsuarioId == filters.userId);
            }
            if (filters.Date != null)
            {
                reservas = reservas.Where(x => x.FechaCreacion.ToShortDateString() == filters.Date?.ToShortDateString());
            }
            if (filters.Description != null)
            {
                reservas = reservas.Where(x => x.Notas.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<object>.Create(reservas, filters.PageNumber, filters.PageSize);
            if (pagedPosts.Any())
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Information", Description = "Registros de posts recuperados correctamente" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ResponseData()
                {
                    Messages = new Message[] { new() { Type = "Warning", Description = "No fue posible recuperar la cantidad de registros" } },
                    Pagination = pagedPosts,
                    StatusCode = HttpStatusCode.OK
                };
            }


        }

        public async Task<Reservas> GetReservaAsync(int id)
        {
            var reserva = await _unitOfWork.ReservaRepository.GetById(id);
            if (reserva == null)
                throw new Exception("Reserva no encontrada");


            return reserva;

        }

        public async Task<IEnumerable<ReservaQueryFilter>> GetReservas24Async()
        {
            var reservas = await _unitOfWork.ReservaRepository.GetAllReservas24DapperAsync();
            
            return reservas;

        }

        public async Task<IEnumerable<NroReservasPorUsuario>> GetNroReservasPorUsuarioAsync()
        {
            var reservas = await _unitOfWork.ReservaRepository.GetNroReservasPorUsuarioAsync();

            return reservas;

        }


        public async Task<IEnumerable<ReservasConPagoRealizado>> GetReservasConPagoRealizadoAsync()
        {
            var reservas = await _unitOfWork.ReservaRepository.GetReservasConPagoRealizadoAsync();

            return reservas;

        }

        public async Task InsertReservaAsync(Reservas reserva)
        {
            /* var overlapping = await _reservaRepository.GetOverlappingReservationsAsync(reserva.CanchaId, reserva.FechaInicio, reserva.FechaFin);
             if (overlapping.Any())
                 throw new Exception("La cancha ya está reservada en ese horario");
            */
            var usuario = await _unitOfWork.UsuarioRepository.GetById(reserva.UsuarioId);
            if (usuario == null)
            {
                throw new BussinesException("El usuario no existe");
            }
            var cancha = await _unitOfWork.CanchaRepository.GetById(reserva.CanchaId);
            if (usuario == null)
            {
                throw new BussinesException("La cancha no existe");
            }
            reserva.Estado = "Reservada";

            reserva.FechaCreacion = DateTime.Now;

            await _unitOfWork.ReservaRepository.Add(reserva);
        }

        public async Task UpdateReservaAsync(Reservas reserva)
        {
            var existingReserva = await _unitOfWork.ReservaRepository.GetById(reserva.Id);
            if (existingReserva == null)
                throw new BussinesException("Reserva no encontrada");

            await _unitOfWork.ReservaRepository.Update(reserva);
        }

        public async Task DeleteReservaAsync(Reservas reserva)
        {
            var existingReserva = await _unitOfWork.ReservaRepository.GetById(reserva.Id);
            if (existingReserva == null)
                throw new BussinesException("Reserva no encontrada");

            await _unitOfWork.ReservaRepository.Delete(reserva);
        }
    }
}
