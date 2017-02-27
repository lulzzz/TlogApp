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
            /// Log
            /// </summary>
            public IActorRef LogActor { get; private set; }

            public StartConversion(TlogFileModel file, IActorRef logActor)
            {
                File = file;
                LogActor = logActor;
            }
        }

        public class StartReader
        {
            /// <summary>
            /// Datos a leer
            /// </summary>
            public enum Reader { Sales }

            public Reader DataToRead { get; private set; }

            /// <summary>
            /// Tlog convertido a leer
            /// </summary>
            public string TlogFilePath { get; private set; }

            /// <summary>
            /// Actor a quien reporta
            /// </summary>
            public IActorRef LogActor { get; private set; }

            public StartReader(string tlogFilePath, Reader dataToRead, IActorRef logActor)
            {
                LogActor = logActor;
                TlogFilePath = tlogFilePath;
                DataToRead = dataToRead;
            }
        }

        #endregion

        protected override void OnReceive(object message)
        {
            if(message is StartConversion)
            {
                var msg = message as StartConversion;
                
                Context.ActorOf(Props.Create(() => new TlogConvertActor(msg.File, msg.LogActor)));

            }else if(message is StartReader)
            {
                var msg = message as StartReader;
                IActorRef tlogReaderActor = Context.ActorOf(Props.Create(() => new TlogReaderActor(msg.LogActor)));

                if (msg.DataToRead == StartReader.Reader.Sales)
                {
                    tlogReaderActor.Tell(new TlogReaderActor.Sales(msg.TlogFilePath));
                }
            }

        }
        
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(x => { return Directive.Restart; });
        }
    }
}
