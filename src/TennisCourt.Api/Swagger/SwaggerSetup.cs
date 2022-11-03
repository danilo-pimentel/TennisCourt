using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace TennisCourt.Api.Swagger
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "Tennis Courts",
                    Version = "v1"
                };

                c.SwaggerDoc(name: "v1", info: openApiInfo);
                c.IncludeXmlComments(AppContext.BaseDirectory + $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(AppContext.BaseDirectory + "TennisCourt.Application.xml");


            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }
    }
}