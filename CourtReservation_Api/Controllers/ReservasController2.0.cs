using AutoMapper;
using CourtReservation_Api.Responses;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Enum;
using CourtReservation_Core.Interfaces.Services_Interfaces;
using CourtReservation_Core.QueryFilters;
using CourtReservation_Infraestructure.Dto_s;
using CourtReservation_Infraestructure.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;


namespace CourtReservation_Api.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReservasController2 : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Version = "v2",
                Message = "Obteniendo publicaciones (versión 2 mejorada)",
                Changes = "Ahora incluye comentarios y autor"
            });
        }


    }
     
}
