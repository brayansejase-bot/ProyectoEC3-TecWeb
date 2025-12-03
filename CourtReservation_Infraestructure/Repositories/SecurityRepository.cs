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
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        private readonly ApplicationDbContext _context;
        public SecurityRepository(ApplicationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Security> GetLoginByCredentials(UserLogin login)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Login == login.Login);
        }
    }

}
