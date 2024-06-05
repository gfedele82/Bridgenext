using Bridgenext.Models.Configurations;
using Microsoft.OpenApi.Models;

namespace Bridgenext.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void InitializeSwagger(this IApplicationBuilder app, string domain)
        {
            var swaggerVersion = SystemParameters.SwaggerVersion;
            app.UseSwagger(
                options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = new List<OpenApiServer>()
                        {
                            new OpenApiServer() { Url = $"https://{httpReq.Host}" },
                            new OpenApiServer() { Url = $"http://{httpReq.Host}" }
                        };
                    });
                });
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{swaggerVersion}/swagger.json", $"{domain} {swaggerVersion}"));
        }
    }
}
