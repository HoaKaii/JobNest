using System.Web.Http;
using WebActivatorEx;
using JobsFinder_Main;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace JobsFinder_Main
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c => c.SingleApiVersion("v1", "JobsFinder_Main"))
                .EnableSwaggerUi();
        }
    }
}
