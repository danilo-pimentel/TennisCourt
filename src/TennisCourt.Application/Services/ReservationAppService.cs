using AutoMapper;
using MassTransit;
using Ns.Contracts.Commons;
using Ns.Contracts.EntryPoint;
using TennisCourt.Application.Interface;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.CrossCutting.Commons.Extensions;

namespace TennisCourt.Application.Services
{
    public class ReservationAppService : IReservationAppService
    {

        private readonly IReservationRepository _repository;
        public ReservationAppService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public Task<Reservation> CancelReservation(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> GetReservation(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> ProcessReservation(Reservation reservation)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation> RescheduleReservation(Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}