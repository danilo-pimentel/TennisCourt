using AutoMapper;
using Polly;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Interface;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.CrossCutting.Commons.Extensions;
using AutoMapper.QueryableExtensions;

namespace TennisCourt.Application.Services
{
    public class ReservationAppService : IReservationAppService
    {

        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _map;

        public ReservationAppService(IReservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _map = mapper.ConfigurationProvider;
        }

        public async Task<IEnumerable<ReservationResponseDTO>> GetAllReservations()
        {
            return _repository.GetAllQuery.ProjectTo<ReservationResponseDTO>(_map);
        }

        public async Task<ReservationResponseDTO> GetReservation(ReservationDetailDTO reservationDetail)
        {
            var existingReservation = _repository.GetAllQuery.FirstOrDefault(r => r.Id.Equals(reservationDetail.Id));
            if (existingReservation == null) return null;

            return _mapper.Map<ReservationResponseDTO>(existingReservation);
        }

        public async Task<ReservationResponseDTO> ProcessReservation(ReservationCreateDTO createReservation)
        {
            var existingReservation = await _repository.GetAvailableScheduleAsync(createReservation.Date);
            if (existingReservation != null) return null;

            var reservation = _mapper.Map<Reservation>(createReservation);

            reservation.ReservationStatus = Domain.Enums.ReservationStatusEnum.READY_TO_PLAY;

            var reservationResult = await _repository.AddAsync(reservation);

            return _mapper.Map<ReservationResponseDTO>(reservationResult);
        }

        public async Task<ReservationResponseDTO> CancelReservation(ReservationCancelDTO reservationCancel)
        {
            var existingReservation = _repository.GetAllQuery.FirstOrDefault(r => r.Id.Equals(reservationCancel.Id));

            if (existingReservation == null || 
                existingReservation.ReservationStatus == Domain.Enums.ReservationStatusEnum.CANCELED) return null;

            existingReservation.ReservationStatus = Domain.Enums.ReservationStatusEnum.CANCELED;
            existingReservation.RefundValue = reservationCancel.RefundValue;

            await _repository.Update(existingReservation);

            return _mapper.Map<ReservationResponseDTO>(existingReservation);
        }

        public async Task<ReservationResponseDTO> RescheduleReservation(ReservationRescheduleDTO reservationReschedule)
        {
            var existingReservation = _repository.GetAllQuery.FirstOrDefault(r => r.Id.Equals(reservationReschedule.Id));

            if (existingReservation == null ||
                existingReservation.ReservationStatus == Domain.Enums.ReservationStatusEnum.CANCELED) return null;

            var existingReservationDate = await _repository.GetAvailableScheduleDifferFromAsync(reservationReschedule.NewDate, reservationReschedule.Id);

            if (existingReservationDate != null) return null;

            existingReservation.Date = reservationReschedule.NewDate;

            await _repository.Update(existingReservation);

            return _mapper.Map<ReservationResponseDTO>(existingReservation);
        }
    }
}
