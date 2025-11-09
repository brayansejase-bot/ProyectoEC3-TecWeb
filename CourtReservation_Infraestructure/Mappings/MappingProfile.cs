using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CourtReservation_Core.Entities;
using CourtReservation_Core.QueryFilters;
using CourtReservation_Infraestructure.Dto_s;


namespace CourtReservation_Infraestructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Reservas, ReservaDto>();
            CreateMap<ReservaDto, Reservas>();
            CreateMap<ReservaQueryFilter, ReservaDto>();
        }
    }
}
