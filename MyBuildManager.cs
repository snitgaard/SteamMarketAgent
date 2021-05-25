using System;
using System.Collections.Generic;
using System.Text;

namespace SteamMarketAgent
{
    class MyBuildManager
    {
        private Dictionary<string, BuildAgent> agents = new Dictionary<string, BuildAgent>();
        private Dictionary<string, int> agentLocks = new Dictionary<string, int>();

        public void AddAgent(string agentId, string solutionPath, int intervalSeconds)
        {
            if (agents.ContainsKey(agentId))
            {
                throw new ArgumentException("Agent id already in use [{0}]", agentId);
            }

            // all locks on build paths are in lowercase.
            solutionPath = solutionPath.ToLower();

            // get the lock for this build path. If no lock exists then create one
            // note: ints are used as lock objects, so we can count the number of agents
            // using it.
            if (!agentLocks.ContainsKey(solutionPath))
            {
                agentLocks.Add(solutionPath, 0);
            }

            // increasing the lock count since we now have one more agent.
            var agentLock = ++agentLocks[solutionPath];

            // creating the agent with the lock object
            agents[agentId] = new BuildAgent(agentId, solutionPath, intervalSeconds, agentLock);
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

            // decreasing the agent count for this build path
            var agentLock = --agentLocks[agent.BuildPath.ToLower()];

            // if no agent on this build path, then remove the lock for this build path
            if (agentLock == 0)
            {
                agentLocks.Remove(agent.BuildPath.ToLower());
            }

            // and of course remove the agent it selv from the manager.
            agents.Remove(agentId);
        }
    }
}
