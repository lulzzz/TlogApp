using Akka.Actor;
using Tlog.Models.Logger;

namespace Tlog.Models.Actors
{
    class LoggerActor : UntypedActor
    {
        #region Messages types

        /// <summary>
        /// Escribe en el log
        /// </summary>
        public class WriteLog
        {
            public string Log { get; private set; }

            public LogType Type { get; private set; }

            public WriteLog(string log, LogType type)
            {
                Log = log;
                Type = type;
            }
        }

        #endregion

        private readonly ILogger _logger;

        public LoggerActor(ILogger logger)
        {
            _logger = logger;
        }

        protected override void OnReceive(object message)
        {
            if(message is WriteLog)
            {
                var msg = message as WriteLog;

                _logger.Log(msg.Log, msg.Type);
            }
        }
    }
}
