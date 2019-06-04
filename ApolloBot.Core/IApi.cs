using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApolloBot.Core
{
    public interface IApi
    {
        void Init(IConfigurationRoot configuration);
        IEnumerable<BotAction> GetActions();
        IEnumerable<BotReader> GetReaders();
    }
}
