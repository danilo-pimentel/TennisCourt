using TennisCourt.Domain.Models;
using System.Linq.Expressions;


namespace TennisCourt.Domain.Interfaces.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<Reservation> GetAvailableScheduleAsync(DateTime date);

        Task<Reservation> GetAvailableScheduleDifferFromAsync(DateTime date, Guid id);

    }
}
