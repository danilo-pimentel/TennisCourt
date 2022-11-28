using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Domain.Enums;

namespace TennisCourt.Application.DTO
{
    public class ReservationResponseDTO
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public string ReservationStatus { get; set; }
        public decimal RefundValue { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

    }
}
