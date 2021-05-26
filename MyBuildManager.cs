using System;
using System.Collections.Generic;
using System.Text;

namespace SteamMarketAgent
{
    class MyBuildManager
    {
        private Dictionary<string, BuildAgent> agents = new Dictionary<string, BuildAgent>();

        public void AddAgent(string agentId, int intervalSeconds, string urlString, string desiredPrice, string emailTo)
        {
            if (agents.ContainsKey(agentId))
            {
                throw new ArgumentException("Agent id already in use [{0}]", agentId);
            }
            agents[agentId] = new BuildAgent(agentId, intervalSeconds, urlString,  desiredPrice, emailTo);
        }

        public void RemoveAgent(string agentId)
        {
            if (!agents.ContainsKey(agentId))
            {
                throw new ArgumentException("Unknown Agent id [{0}]", agentId);
            }
            BuildAgent agent = agents[agentId];
            agent.Cancel();
            agents.Remove(agentId);
        }
    }
}

