using AutoMapper;
using CourtReservation_Api.Responses;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Enum;
using CourtReservation_Core.Interfaces;
using CourtReservation_Core.Services;
using CourtReservation_Infraestructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourtReservation_Api.Controllers
{

       // [Authorize(Roles = nameof(RoleType.Administrator))]
     //   [Produces("application/json")]
        [Route("api/[controller]")]
        [ApiController]

      // [Authorize(Roles = $"{nameof(RoleType.Administrator)},{nameof(RoleType.Consumer)}")]

   //     [Authorize(Roles = nameof(RoleType.Administrator))]
    //    [Authorize(Roles = nameof(RoleType.Supervisor))]

    public class SecurityController : ControllerBase
        {
            private readonly ISecurityService _securityService;
            private readonly IMapper _mapper;
            private readonly IPasswordService _passwordService;

            public SecurityController(ISecurityService securityService,
                IMapper mapper, IPasswordService passwordService)
            {
                _securityService = securityService;
                _mapper = mapper;
                _passwordService = passwordService;
            }



            [Authorize(Roles = nameof(RoleType.Administrator))]
            [HttpGet]
            public async Task<IActionResult> Get()
            {
                Dictionary<string, string> dic = new();
                dic.Add("Message", "Coneccion exitosa");
                return Ok(dic);
            }   

            [HttpPost]
            public async Task<IActionResult> Post(SecurityDto securityDto)
            {
                var security = _mapper.Map<Security>(securityDto);
                security.Password = _passwordService.Hash(security.Password);
                await _securityService.RegisterUser(security);

                securityDto = _mapper.Map<SecurityDto>(security);
                var response = new ApiResponse<SecurityDto>(securityDto);
                return Ok(response);
            }
        }

}
