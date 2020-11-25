using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace blogprojectabp
{
    public class blogprojectabpWebTestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<blogprojectabpWebTestModule>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.InitializeApplication();
        }
    }
}