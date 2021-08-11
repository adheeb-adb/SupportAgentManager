namespace SupportAgentManager.Domain.Constants
{
    public static class ConfigNames
    {
        public static readonly string CloudStorageInformation = "CloudStorageInformation";

        public static readonly string AgentConfigInformation = "AgentConfigInformation";
        public static readonly string ReserveAgentEfficiency = $"{AgentConfigInformation}:ReserveAgentEfficiency";
        public static readonly string JuniorAgentEfficiency = $"{AgentConfigInformation}:JuniorAgentEfficiency";
        public static readonly string MidLevelAgentEfficiency = $"{AgentConfigInformation}:MidLevelAgentEfficiency";
        public static readonly string SeniorAgentEfficiency = $"{AgentConfigInformation}:SeniorAgentEfficiency";
        public static readonly string LeadAgentEfficiency = $"{AgentConfigInformation}:LeadAgentEfficiency";

        public static readonly string ChatSessionQueueInformation = "ChatSessionQueueInformation";
        public static readonly string ShiftInformation = "ShiftInformation";
        public static readonly string BusinessShift = $"{ShiftInformation}:BusinessShift";
        public static readonly string MidShift = $"{ShiftInformation}:MidShift";
        public static readonly string NightShift = $"{ShiftInformation}:NightShift";
    }
}
