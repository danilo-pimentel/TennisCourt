using Microsoft.Extensions.DependencyInjection;
using TennisCourt.Application.AutoMapper;
using System;
using TennisCourt.Infra.CrossCutting.Commons.Providers;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Microsoft.Extensions.Options;

namespace TennisCourt.Api.Configurations
{
    public static class UserProvidedSettingsLoader
    {
        public static void ConfigureUserProvidedSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                var serviceOptions = configuration.GetSection("vcap").Get<CloudFoundryServicesOptions>();
                var userProvidedService = serviceOptions.Services["user-provided"].First(s => s.Name == "NsUserProvided");
                return Options.Create(new UserProvidedSettingsProvider() { ConnectionString = userProvidedService.Credentials["connectionString"].Value });
            });
        }
    }

}
