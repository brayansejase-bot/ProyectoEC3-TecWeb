using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CourtReservation_Infraestructure.Queries
{
    public static class ReservaQueries
    {
        public static string ReservaQuery24SqlServer = @"
                        SELECT 
                           UsuarioId AS userId,
                    FechaCreacion AS Date,
                    Notas AS Description
                    FROM 
                        Reservas
                    WHERE 
                        YEAR(FechaCreacion) = 2024
                    ORDER BY FechaCreacion ASC
                    OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                    ";
                    

        public static string NroReservasPorUsuarioSqlServer = @"
                        SELECT 
                           UsuarioId 
                    COUNT(*) AS ReservationCount
                    FROM 
                        Reservas
                    GROUP BY 
                        UsuarioId
                    ORDER BY 
                        ReservationCount DESC
                    OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
                    ";

        public static string ReservasConPagoRealizado = @"
                        SELECT r.Id, r.FechaInicio, r.FechaFin, r.Notas,
                  u.Nombre AS UsuarioNombre, c.Nombre AS CanchaNombre,
                    p.Monto, p.EstadoPago
                    FROM Reservas r
                    JOIN Usuarios u ON r.UsuarioId = u.Id
                    JOIN Canchas c ON r.CanchaId = c.Id
                    JOIN Pagos p ON p.ReservaId = r.Id
                    WHERE r.Estado = 'Completada' AND p.EstadoPago = 'Pagado'
                    ORDER BY r.FechaFin DESC;

                    ";

        public static string PostQueryMySQl = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        LIMIT @Limit
                    ";
    }
}
