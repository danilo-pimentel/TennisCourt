using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Domain.Enums;

namespace TennisCourt.Application.DTO
{
    public class ReservationRescheduleDTO
    {
        public Guid Id { get; set; }
        public DateTime NewDate { get; set; }

    }
}
