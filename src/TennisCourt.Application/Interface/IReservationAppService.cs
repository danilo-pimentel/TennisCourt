using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Application.DTO;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;

namespace TennisCourt.Application.Interface
{
    public interface IReservationAppService
    {
        Task<IEnumerable<ReservationResponseDTO>> GetAllReservations();
        Task<ReservationResponseDTO> GetReservation(ReservationDetailDTO reservation);
        Task<ReservationResponseDTO> ProcessReservation(ReservationCreateDTO reservation);
        Task<ReservationResponseDTO> CancelReservation(ReservationCancelDTO reservation);
        Task<ReservationResponseDTO> RescheduleReservation(ReservationRescheduleDTO reservation);
    }
}
