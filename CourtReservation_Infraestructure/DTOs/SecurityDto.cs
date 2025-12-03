using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourtReservation_Core.Enum;

namespace CourtReservation_Infraestructure.DTOs
{
    public class SecurityDto
    {

        public string Password { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public string Login { get; set; }

    }

}
