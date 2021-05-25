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


            // creating the agent with the lock object
            agents[agentId] = new BuildAgent(agentId, intervalSeconds, urlString,  desiredPrice, emailTo);
        }

        public void RemoveAgent(string agentId)
        {
            // is the agentID valid ?
            if (!agents.ContainsKey(agentId))
            {
                throw new ArgumentException("Unknown Agent id [{0}]", agentId);
            }

            // cancel the tread for this agent
            BuildAgent agent = agents[agentId];
            agent.Cancel();


            // and of course remove the agent it selv from the manager.
            agents.Remove(agentId);
        }
    }
}

