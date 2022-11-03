using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TennisCourt.Infra.CrossCutting.Commons.Extensions;
using TennisCourt.Infra.CrossCutting.Commons.Providers;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace TennisCourt.Api.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserProvidedSettingsProvider _userProvided;

        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("")]
        [Route("home")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("globalError/{errorMessage}")]
        public IActionResult GlobalError(string errorMessage)
        {
            throw new Exception(errorMessage);
        }
    }
}