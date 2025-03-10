﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Models.Base;

namespace TennisCourt.Domain.Models
{
    public class Reservation : BaseEntity
    {
        public decimal Value { get;set; }
        public ReservationStatusEnum ReservationStatus { get; set; }
        public decimal RefundValue { get; set; }
        public DateTime Date { get; set; }
        
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(30)]
        public string Phone { get; set; }
    }
}
