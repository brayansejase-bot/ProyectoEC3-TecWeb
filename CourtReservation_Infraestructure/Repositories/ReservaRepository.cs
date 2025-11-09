using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Enum;
using CourtReservation_Core.Interfaces;
using CourtReservation_Core.QueryFilters;
using CourtReservation_Infraestructure.Context;
using CourtReservation_Infraestructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace CourtReservation_Infraestructure.Repositories
{
    public class ReservaRepository : BaseRepository<Reservas>, IReservaRepository
    {

        private readonly IDapperContext _dapper;
        //private readonly SocialMediaContext _context;
        public ReservaRepository(ApplicationDbContext context,
            IDapperContext dapper) : base(context)
        {
            //_context = context;
            _dapper = dapper;
        }


        public async Task<IEnumerable<ReservaQueryFilter>> GetAllReservas24DapperAsync(int limit = 10)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DatabaseProvider.SqlServer => ReservaQueries.ReservaQuery24SqlServer,

                    DatabaseProvider.MySql => ReservaQueries.PostQueryMySQl,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<ReservaQueryFilter>(sql, new { Limit = limit });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<NroReservasPorUsuario>> GetNroReservasPorUsuarioAsync()
        {
            try
            {
                var sql = ReservaQueries.NroReservasPorUsuarioSqlServer;

                return await _dapper.QueryAsync<NroReservasPorUsuario>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        public async Task<IEnumerable<ReservasConPagoRealizado>> GetReservasConPagoRealizadoAsync()
        {
            try
            {
                var sql = ReservaQueries.ReservasConPagoRealizado;

                return await _dapper.QueryAsync<ReservasConPagoRealizado>(sql);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

    }




    
}
