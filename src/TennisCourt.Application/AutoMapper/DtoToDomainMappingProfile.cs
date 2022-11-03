using AutoMapper;
using Ns.Contracts.Commons;
using Ns.Contracts.EntryPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisCourt.Application.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {

            //CreateMap<RequestWrapper<SmsRequestDto>, ExecutionRequest>()
            //    .IncludeMembers(s => s.Data)
            //    .AfterMap((src, dest) =>
            //    {
            //        src.Data.ExecutionId = src.ExecutionId;
            //        dest.Payload = src.Data.ToJson();
            //        dest.FlowType = "SmsFulfillment"; //TODO: VERIFICAR FLOWTYPE ENUM
            //    });
        }
    }
}
