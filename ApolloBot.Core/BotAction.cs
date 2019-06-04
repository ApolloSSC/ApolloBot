using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApolloBot.Core
{
    public class BotAction
    {
        public string CommandLine { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ExecuteDelegate Execute { get; set; }

        public delegate Task<string> ExecuteDelegate(string[] parameters, Action<string> log, BotReader reader, string user = null, DateTime? time = null);
    }
}
