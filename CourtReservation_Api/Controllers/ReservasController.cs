using AutoMapper;
using CourtReservation_Api.Responses;
using CourtReservation_Core.CustomEntities;
using CourtReservation_Core.Entities;
using CourtReservation_Core.Enum;
using CourtReservation_Core.Interfaces.Services_Interfaces;
using CourtReservation_Core.QueryFilters;
using CourtReservation_Infraestructure.Dto_s;
using CourtReservation_Infraestructure.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;


namespace CourtReservation_Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ReservasController : ControllerBase
    {
        
        private readonly IReservaService _service; // Inyecta el servicio
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public ReservasController(IReservaService service,
                                  IMapper mapper, 
                                  IValidationService validationService)
        {
            _service = service;
            _mapper = mapper;
            _validationService = validationService;
        }


        /// <summary>
        /// Recupera todas las reservas con paginación y filtros opcionales.
        /// </summary>
        /// <remarks>Este método utiliza un mapeador para convertir las entidades de reserva en DTOs antes de devolverlos al cliente.</remarks>
        /// <param name="filters"> Filtros de consulta para paginación y búsqueda.</param>
        /// <returns> Lista paginada de reservas.</returns>
        /// <response code="200"> Retorna la lista paginada de reservas.</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        public async Task<IActionResult> GetAllReservas([FromQuery] ReservaQueryFilter reservaQueryFilter)
        {
            try
            {
                var reservas = await _service.GetAllReservasAsync(reservaQueryFilter);

                var reservasDto = _mapper.Map<IEnumerable<ReservaDto>>(reservas.Pagination);

                var pagination = new Pagination
                {
                    TotalCount = reservas.Pagination.TotalCount,
                    PageSize = reservas.Pagination.PageSize,
                    CurrentPage = reservas.Pagination.CurrentPage,
                    TotalPages = reservas.Pagination.TotalPages,
                    HasNextPage = reservas.Pagination.HasNextPage,
                    HasPreviousPage = reservas.Pagination.HasPreviousPage
                };
                var response = new ApiResponse<IEnumerable<ReservaDto>>(reservasDto)
                {
                    Pagination = pagination,
                    Messages = reservas.Messages
                };

                return StatusCode((int)reservas.StatusCode, response);
            }
            catch (Exception err)
            {
                var responsePost = new ResponseData()
                {
                    Messages = new Message[] { new() { Type = TypeMessage.error.ToString(), Description = err.Message } },
                };
                return StatusCode(500, responsePost);
            }

        }

        /// <summary>
        /// Recupera una reserva por su ID.
        /// </summary>
        /// <remarks> Este método valida el ID proporcionado antes de intentar recuperar la reserva correspondiente.</remarks> 
        /// <param name="id">ID de la reserva a recuperar.</param>"
        /// <returns> La reserva correspondiente al ID proporcionado.</returns>
        /// <response code="200"> Retorna la reserva</response>
        /// <response code="400">ID de reserva inválido</response>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserva(int id)
        {
                var validationRequest = new GetByIdRequest { Id = id };
                var validationResult = await _validationService.ValidateAsync(validationRequest);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Error de validación del ID",
                        Errors = validationResult.Errors
                    });
                }

                var reserva = await _service.GetReservaAsync(id);
                var reservatDto = _mapper.Map<ReservaDto>(reserva);

                ApiResponse<ReservaDto> response = new ApiResponse<ReservaDto>(reservatDto);
                
                return Ok(response);

        }

        /// <summary>
        /// Recupera las reservas realizadas pertenecientes al año 2024.
        /// </summary>
        /// <remarkarks> Este método utiliza Dapper para ejecutar una consulta optimizada que recupera las reservas del año 2024.</remarks>
        /// <param name="reservaQueryFilter">Filtros de consulta (no utilizados en esta implementación).</param>"
        /// <returns> Lista de reservas del año 2024.</returns>
        /// <response code="200"> Retorna la lista de reservas del año 2024.</response>
        [HttpGet("dapper/24")]
        public async Task<IActionResult> GetReservaByUserId([FromQuery] ReservaQueryFilter reservaQueryFilter)
        {

            var posts = await _service.GetReservas24Async();


            var response = new ApiResponse<IEnumerable<ReservaQueryFilter>>(posts);

            return Ok(response);
        }

        /// <summary> 
        /// Recupera el número de reservas por usuario.
        /// </summary>
        /// <remarks> Este método utiliza Dapper para ejecutar una consulta optimizada que cuenta las reservas por usuario.</remarks> 
        /// <param  name="reservaQueryFilter">Filtros de consulta (no utilizados en esta implementación).</param>"
        /// <returns> Lista del número de reservas por usuario.</returns>
        /// <response code="200"> Retorna la lista del número de reservas por usuario.</response>
        /// <response code="500">Error interno del servidor</response>

        [HttpGet("dapper/2")]
        public async Task<IActionResult> GetNroReservaByUser([FromQuery] NroReservasPorUsuario reservaQueryFilter)
        {

            var posts = await _service.GetNroReservasPorUsuarioAsync();


            var response = new ApiResponse<IEnumerable<NroReservasPorUsuario>>(posts);

            return Ok(response);
        }

        /// <summary>
        /// Recupera las reservas con pago realizado.
        /// </summary>
        /// <remarks > Este método utiliza Dapper para ejecutar una consulta optimizada que recupera las reservas con pago realizado.</remarks> 
        /// <param name="reservaQueryFilter">Filtros de consulta (no utilizados en esta implementación).</param>"
        /// <returns > Lista de reservas con pago realizado.</returns>
        /// <response code="200" > Retorna la lista de reservas con pago realizado.</response>
        [HttpGet("dapper/3")]
        public async Task<IActionResult> GetReservasPagoRealizado([FromQuery] ReservasConPagoRealizado reservaQueryFilter)
        {

            var posts = await _service.GetReservasConPagoRealizadoAsync();


            var response = new ApiResponse<IEnumerable<ReservasConPagoRealizado>>(posts);

            return Ok(response);
        }


        /// <summary>
        /// Se utiliza para crear una nueva reserva.
        /// </summary>
        /// <remarks> Este método valida el DTO de reserva antes de crear la nueva reserva en el sistema.</remarks> 
        /// <param  name="reservaDto">DTO de reserva que contiene los datos de la nueva reserva.</param>"   "
        /// <returns> La reserva creada.</returns>
        /// <response code="200"> Retorna la reserva creada.</response>
        /// <response code="500">Error interno del servidor</response>

        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaDto reservaDto)
        {
           
                #region Validaciones
              
                var validationResult = await _validationService.ValidateAsync(reservaDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { Errors = validationResult.Errors });
                }
                #endregion

                var reserva = _mapper.Map<Reservas>(reservaDto); 

                 await _service.InsertReservaAsync(reserva);

                var response = new ApiResponse<ReservaDto>(reservaDto)
                {
                    Data = reservaDto,
                    Message = "Reserva creada exitosamente",
                    Success = true,
                    StatusCode = 201
                };
                return StatusCode(201, response); 
        }

        /// <summary>
        /// Sirve para actualizar una reserva existente.
        /// </summary>
        /// <remarks> Este método valida el DTO de reserva antes de actualizar la reserva en el sistema.</remarks> 
        /// <param name="reservaDto">DTO de reserva que contiene los datos actualizados de la reserva.</param>""
        /// <returns>  Reserva actualizada.</returns>
        /// <response code="200"> Retorna la reserva actualizada.</response>
        /// <response code="500">Error interno del servidor</response>

        [HttpPut]
        public async Task<IActionResult> ActualizarReserva([FromBody] ReservaDto reservaDto)
        {
            if (reservaDto == null)
                return BadRequest(new { Errors = new List<string> { "El body no puede ser nulo. Envíe un JSON válido." } });

            var validationResult = await _validationService.ValidateAsync(reservaDto);

            if (!validationResult.IsValid)
                return BadRequest(new { Errors = validationResult.Errors });

            var reserva = _mapper.Map<Reservas>(reservaDto);

            await _service.UpdateReservaAsync(reserva);

            var response = new ApiResponse<ReservaDto>(reservaDto)
            {
                Data = reservaDto,
                Message = "Reserva actualizada exitosamente",
                Success = true,
                StatusCode = 200
            };
            return Ok(response);
        }


        /// <summary>
        /// Sirve para borrar una reserva existente.
        /// </summary>
        /// <remarks> Este método valida el DTO de reserva antes de borrar la reserva en el sistema.</remarks>
        /// <param name="reservaDto">DTO de reserva que contiene los datos de la reserva a borrar.</param>""
        /// <returns > NoContent si la reserva fue borrada exitosamente.</returns>
        /// <response code="204"> NoContent si la reserva fue borrada exitosamente.</response>
        [HttpDelete]
        public async Task<IActionResult> BorrarReserva([FromBody] ReservaDto reservaDto)
        {

            #region Validaciones

            var validationResult = await _validationService.ValidateAsync(reservaDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors });
            }
            #endregion

            var reserva = _mapper.Map<Reservas>(reservaDto);

            await _service.DeleteReservaAsync(reserva);

            return NoContent();
        }
    }
}
