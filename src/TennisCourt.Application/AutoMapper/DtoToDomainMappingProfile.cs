using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisCourt.Application.DTO;
using TennisCourt.Domain.Models;

namespace TennisCourt.Application.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<ReservationResponseDTO, Reservation>();
            CreateMap<ReservationCreateDTO, Reservation>();
            CreateMap<ReservationCancelDTO, Reservation>();
            CreateMap<ReservationRescheduleDTO, Reservation>();
        }
    }
}
