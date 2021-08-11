using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportAgentManager.Common.Domain.Contracts.CloudStorage;
using SupportAgentManager.Common.Domain.Dto.CloudStorage;
using SupportAgentManager.Common.Services.CloudStorage;
using SupportAgentManager.Domain.Constants;
using SupportAgentManager.Domain.Contracts;
using SupportAgentManager.Domain.Dto.ConfigInformation;
using SupportAgentManager.Domain.Enums;
using SupportAgentManager.Services;
using System.Collections.Generic;

[assembly: FunctionsStartup(typeof(SupportAgentManager.Functions.Startup))]

namespace SupportAgentManager.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = builder.GetContext().Configuration;

            builder.Services.AddSingleton(configuration.GetSection(ConfigNames.CloudStorageInformation).Get<CloudStorageInformation>());
            builder.Services.AddSingleton<ICloudQueueClientFactory, CloudQueueClientFactory>();
            builder.Services.AddTransient<IAzureQueueService, AzureQueueService>();

            builder.Services.AddSingleton(GetAgetnConfigInformation(configuration));
            builder.Services.AddSingleton(configuration.GetSection(ConfigNames.ChatSessionQueueInformation).Get<ChatSessionQueueInformation>());
            builder.Services.AddSingleton(GetShiftsConfiguration(configuration));
            builder.Services.AddSingleton<ISupportShiftService, SupportShiftService>();
            builder.Services.AddSingleton<IAgentService, AgentService>();
            builder.Services.AddSingleton<IAgentQueueService, AgentQueueService>();
            builder.Services.AddSingleton<IChatSessionQueueService, ChatSessionQueueService>();
            builder.Services.AddSingleton<IChatSessionService, ChatSessionService>();

            builder.Services.AddScoped<IAgentChatCoordinatorService, AgentChatCoordinatorService>();


        }

        #region: private methods

        // TODO: Move to a config helper service, so that this may be used by other startup methods as well.

        private AgentConfigInformation GetAgetnConfigInformation(IConfiguration configuration)
        {
            double reserveAgentEfficiency = configuration.GetSection(ConfigNames.ReserveAgentEfficiency).Get<double>();
            double juniorAgentEfficiency = configuration.GetSection(ConfigNames.JuniorAgentEfficiency).Get<double>();
            double midLevelAgentEfficiency = configuration.GetSection(ConfigNames.MidLevelAgentEfficiency).Get<double>();
            double seniorAgentEfficiency = configuration.GetSection(ConfigNames.SeniorAgentEfficiency).Get<double>();
            double leadAgentEfficiency = configuration.GetSection(ConfigNames.LeadAgentEfficiency).Get<double>();

            AgentConfigInformation agentConfigInformation = configuration.GetSection(ConfigNames.AgentConfigInformation).Get<AgentConfigInformation>();

            agentConfigInformation.AgentEfficiencies = new Dictionary<SeniorityLevel, double>
            {
                { SeniorityLevel.Reserve, reserveAgentEfficiency },
                { SeniorityLevel.Junior, juniorAgentEfficiency },
                { SeniorityLevel.MidLevel, midLevelAgentEfficiency },
                { SeniorityLevel.Senior, seniorAgentEfficiency },
                { SeniorityLevel.TeamLead, leadAgentEfficiency }
            };

            return agentConfigInformation;
        }

        private ShiftsConfiguration GetShiftsConfiguration(IConfiguration configuration)
        {
            var businessShift = configuration.GetSection(ConfigNames.BusinessShift).Get<ShiftConfigInformation>();
            var midShift = configuration.GetSection(ConfigNames.MidShift).Get<ShiftConfigInformation>();
            var nightShift = configuration.GetSection(ConfigNames.NightShift).Get<ShiftConfigInformation>();

            return new ShiftsConfiguration { BusinessShift = businessShift, MidShift = midShift, NightShift = nightShift };
        }

        #endregion
    }
}
