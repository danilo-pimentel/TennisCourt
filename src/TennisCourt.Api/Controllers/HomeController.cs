using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TennisCourt.Infra.CrossCutting.Commons.Extensions;
using TennisCourt.Infra.CrossCutting.Commons.Providers;
using Ns.Vault.Providers;
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
        private readonly ServiceAccountProvider _serviceAccount;

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

        [Route("settings")]
        [HttpGet]
        public IActionResult Settings()
        {
            string framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
            string platformVersion = $"{RuntimeInformation.OSDescription} [ {RuntimeInformation.OSArchitecture} ]";


            ViewData["SettingsList"] = new List<(string Key, string Value)>
            {
                ("<<== HealthCheck Api ==>>", "<<====>>"),
                //("DeliveryApi", deliveryApi),
                ("<<== Database Settings ==>>", "<<====>>"),
                ("<<== General Settings ==>>", "<<====>>"),
                ("EnvironmentName", _hostingEnvironment.EnvironmentName),
                ("TargetFrameWork", framework),
                ("PlatformVersion", platformVersion),
                ("Is Development?", _hostingEnvironment.IsDevelopment().ToString()),
                ("Service Account", $"{_serviceAccount.Domain}\\{_serviceAccount.Name}"),
                ("PCF Instance GUID", Environment.GetEnvironmentVariable("CF_INSTANCE_GUID")),
                ("PCF Instance IP", Environment.GetEnvironmentVariable("CF_INSTANCE_IP")),
                ("PCF Instance Index", Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX")),
                ("<<== Rabbit Settings ==>>", "<<====>>")
            };

            return View();
        }

        [HttpPost("globalError/{errorMessage}")]
        public IActionResult GlobalError(string errorMessage)
        {
            throw new Exception(errorMessage);
        }
    }
}