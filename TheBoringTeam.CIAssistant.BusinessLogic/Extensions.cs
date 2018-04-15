using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TheBoringTeam.CIAssistant.BusinessEntities.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Entities;
using TheBoringTeam.CIAssistant.BusinessLogic.Interfaces;
using TheBoringTeam.CIAssistant.DataAccess;
using TheBoringTeam.CIAssistant.DataAccess.Interfaces;

namespace TheBoringTeam.CIAssistant.BusinessLogic
{
    public static class Extensions
    {
        public static void AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDialogFlowBusinessLogic, DialogFlowBusinessLogic>();
            services.AddTransient<IBaseBusinessLogic<User>, BaseBusinessLogic<User>>();
            services.AddTransient<IBaseBusinessLogic<BusinessEntities.Entities.Action>, BaseBusinessLogic<BusinessEntities.Entities.Action>>();
            services.AddTransient<IBaseMongoRepository<User>>(f =>
                new BaseMongoRepository<User>(configuration["mongoConnectionString"],
                    configuration["mongoDatabaseName"],
                    true));
            services.AddTransient<IBaseMongoRepository<BusinessEntities.Entities.Action>>(f =>
                new BaseMongoRepository<BusinessEntities.Entities.Action>(configuration["mongoConnectionString"],
                    configuration["mongoDatabaseName"],
                    true));
        }
    }
}
