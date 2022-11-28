using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Models;
using TennisCourt.Infra.Data.Context;
using TennisCourt.Infra.Data.Repositories.Base;
using System.Linq.Expressions;

namespace TennisCourt.Infra.Data.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(TennisCourtContext context)
            : base(context)
        {
        }

        public async Task<Reservation> GetAvailableScheduleAsync(DateTime date)
        {
            return this.GetAllQuery.FirstOrDefault(r => r.ReservationStatus != Domain.Enums.ReservationStatusEnum.CANCELED &&
                                                        ((date >= r.Date && date < r.Date.AddHours(1)) ||
                                                        (date > r.Date.AddHours(-1) && date <= r.Date)));
        }

        public async Task<Reservation> GetAvailableScheduleDifferFromAsync(DateTime date, Guid id)
        {
            return this.GetAllQuery.FirstOrDefault(r => r.Id != id &&
                                                        r.ReservationStatus != Domain.Enums.ReservationStatusEnum.CANCELED &&
                                                        ((date >= r.Date && date < r.Date.AddHours(1)) ||
                                                        (date > r.Date.AddHours(-1) && date <= r.Date)));
        }
    }
}
