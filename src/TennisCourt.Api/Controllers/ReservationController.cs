using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Interface;
using TennisCourt.Domain.Models;

namespace TennisCourt.Api.Controllers
{
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationAppService _reservationService;
        private readonly IMapper _mapper;

        public ReservationController(IReservationAppService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("detail/${id}")]
        public async Task<IActionResult> GetDetail([FromRoute] Guid id)
        {
            var detailReservation = _mapper.Map<ReservationDetailDTO>(id);
            var reservation = await _reservationService.GetReservation(detailReservation);
            
            if (reservation == null) return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("process")]
        public async Task<IActionResult> ProcessReservations([FromBody] ReservationCreateDTO reservationCreate)
        {
            var reservationResult = await _reservationService.ProcessReservation(reservationCreate);

            if (reservationResult == null) return UnprocessableEntity();

            var reservationResponse = _mapper.Map<ReservationResponseDTO>(reservationResult);
            var uri = "detail/" + reservationResponse.Id.ToString();
            return Created(uri, reservationResponse);

        }

        [HttpPatch]
        [AllowAnonymous]
        [Route("cancel")]
        public async Task<IActionResult> CancelReservations([FromBody] ReservationCancelDTO reservationCancel)
        {
            var reservationResult = await _reservationService.CancelReservation(reservationCancel);

            if (reservationResult == null) return UnprocessableEntity();

            return Ok(reservationResult);
        }

        [HttpPatch]
        [AllowAnonymous]
        [Route("reschedule")]
        public async Task<IActionResult> RescheduleReservations([FromBody] ReservationRescheduleDTO reservationReschedule)
        {
            var reservationResult = await _reservationService.RescheduleReservation(reservationReschedule);

            if (reservationResult == null) return UnprocessableEntity();

            return Ok(reservationResult);
        }
    }
}