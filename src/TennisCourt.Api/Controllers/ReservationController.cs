using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TennisCourt.Application.Interface;
using TennisCourt.Infra.CrossCutting.Commons.Providers;
using Swashbuckle.AspNetCore.Annotations;

namespace TennisCourt.Api.Controllers
{
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationAppService _smsRequestService;

        public ReservationController(IReservationAppService smsRequestService)
        {
            _smsRequestService = smsRequestService;
        }


    }
}