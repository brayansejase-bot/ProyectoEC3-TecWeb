using System;
using System.Collections.Generic;
using CourtReservation_Core.Enum;

namespace CourtReservation_Core.Entities;

public partial class Security : BaseEntity
{
    //  public int Id { get; set; }

    public string Password { get; set; } 

    public string Name { get; set; } 

    public RoleType Role { get; set; }

    public string Login { get; set; }
}
