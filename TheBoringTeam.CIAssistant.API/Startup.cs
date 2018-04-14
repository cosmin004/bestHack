﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheBoringTeam.CIAssistant.BusinessLogic;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TheBoringTeam.CIAssistant.API.Infrastructure;

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
            services.AddAutoMapper();
            services.AddBusinessLogic(Configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireClaim("Bearer");
                });

                options.AddPolicy("Basic", policy =>
                {
                    policy.RequireClaim("Basic");
                    policy.AddAuthenticationSchemes("Basic");

                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBasicAuthenticationMiddleware();
            app.UseTokenBasedAuthenticationMiddleware();

            app.UseCors(builder =>
                builder.WithOrigins("*")
                       .AllowAnyHeader()
            );

            app.UseMvc();
        }
    }
}
