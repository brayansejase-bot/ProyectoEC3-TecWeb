using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Interfaces;
using CourtReservation_Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourtReservation_Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public readonly IBaseRepository<Canchas>? _canchaRepository;
        public readonly IBaseRepository<Usuarios>? _usuarioRepository;
        public readonly IReservaRepository? _reservaRepository;
        private readonly ISecurityRepository? _securityRepository;
        public readonly IDapperContext _dapper;


        private IDbContextTransaction? _efTransaction;
        public UnitOfWork(ApplicationDbContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }


        public IBaseRepository<Canchas> CanchaRepository =>
            _canchaRepository ?? new BaseRepository<Canchas>(_context);

        public IBaseRepository<Usuarios> UsuarioRepository =>
            _usuarioRepository ?? new BaseRepository<Usuarios>(_context);

        public IReservaRepository ReservaRepository =>
            _reservaRepository ?? new ReservaRepository(_context,_dapper);

        public ISecurityRepository SecurityRepository =>
            _securityRepository ?? new SecurityRepository(_context);

        public void Dispose()
        {
            if (_context != null)
            {
                _efTransaction?.Dispose();
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Transacciones
        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // registrar la conexión/tx en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            // Retornamos la conexión subyacente del DbContext
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }
        #endregion

    }

}
