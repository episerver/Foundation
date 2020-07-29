using EPiServer.Logging;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Install
{
    public class InstallProgressMessenger : IProgressMessenger
    {
        private static readonly ILogger _log = LogManager.GetLogger(typeof(InstallProgressMessenger));

        public int CurrentPercentage { get; private set; }
        public IList<InstallMessage> Messages { get; }

        public InstallProgressMessenger() => Messages = new List<InstallMessage>();

        public void AddProgressMessageText(string message, bool error, int percent)
        {
            CurrentPercentage = percent > 0 ? percent : CurrentPercentage;
            Messages.Insert(0, new InstallMessage { TimeStamp = DateTime.Now, Message = message, Error = error });

            if (error)
            {
                _log.Error(message);
            }
            else
            {
                _log.Debug(message);
            }
        }
    }
}
