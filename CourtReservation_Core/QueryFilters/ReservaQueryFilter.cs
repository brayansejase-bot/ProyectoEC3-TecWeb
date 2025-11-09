using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtReservation_Core.QueryFilters
{
    public class ReservaQueryFilter : PaginationQueryFilter
    {
        public int? userId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
    }

}
