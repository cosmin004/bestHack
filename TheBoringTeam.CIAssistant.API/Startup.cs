using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.DataAccess.Interfaces;
using TheBoringTeam.CIAssistant.BusinessLogic.Entities;
using TheBoringTeam.CIAssistant.DataAccess;

namespace TheBoringTeam.CIAssistant.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IBaseBusinessLogic<User>, BaseBusinessLogic<User>>();
            services.AddTransient<IBaseMongoRepository<User>>(f =>
                new BaseMongoRepository<User>(Configuration["mongoConnectionString"],
                    Configuration["mongoDatabaseName"],
                    true));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
