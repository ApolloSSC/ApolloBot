using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApolloBot.Core
{
    public class BotReader
    {
        public string Name { get; set; }

        public PlayDelegate Play { get; set; }
        public StopDelegate Stop { get; set; }
        public SetVolumeDelegate SetVolume { get; set; }

        public delegate Task<bool> PlayDelegate(Action<string> log, string path);
        public delegate Task<bool> StopDelegate(Action<string> log);
        public delegate Task<bool> SetVolumeDelegate(Action<string> log, int volume);
    }
}
