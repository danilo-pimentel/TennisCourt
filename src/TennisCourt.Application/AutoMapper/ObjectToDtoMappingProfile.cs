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
    public class ObjectToDtoMappingProfile : Profile
    {
        public ObjectToDtoMappingProfile()
        {
            CreateMap<Reservation, ReservationResponseDTO>();
            CreateMap<Guid, ReservationDetailDTO>()
                .ConstructUsing(id => new ReservationDetailDTO() { Id = id });

        }
    }
}
