using Microsoft.Extensions.DependencyInjection;
using TennisCourt.Application.AutoMapper;
using System;

namespace TennisCourt.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(DtoToDtoMappingProfile));
            services.AddAutoMapper(typeof(DtoToDomainMappingProfile));

            AutoMapperConfig.RegisterMappings();

            return services;
        }
    }
}
