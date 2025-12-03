using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CourtReservation_Core.Entities;

namespace CourtReservation_Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Canchas> CanchaRepository { get; }
        IBaseRepository<Usuarios> UsuarioRepository { get; }
        IReservaRepository ReservaRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Nuevos miembros para Dapper
        IDbConnection? GetDbConnection();
        IDbTransaction? GetDbTransaction();

    }

}
