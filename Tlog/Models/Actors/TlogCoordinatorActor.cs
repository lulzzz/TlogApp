using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Tlog.Models.Files;

namespace Tlog.Models.Actors
{
    class TlogCoordinatorActor:UntypedActor
    {
        #region Message types

        /// <summary>
        /// Inicar conversion de tlog binario
        /// </summary>
        public class StartConversion
        {
            /// <summary>
            /// Tlog a convertir
            /// </summary>
            public TlogFileModel File { get; private set; }

            /// <summary>
            /// Actor a quien reporta
            /// </summary>
            public IActorRef ReporterActor { get; private set; }

            public StartConversion(TlogFileModel file, IActorRef reporterActor)
            {
                File = file;
                ReporterActor = reporterActor;
            }
        }

        #endregion

        protected override void OnReceive(object message)
        {
            if(message is StartConversion)
            {
                var msg = message as StartConversion;

                Context.ActorOf(Props.Create(() => new TlogConvertActor(msg.File, msg.ReporterActor)));
            }
        }
    }
}
