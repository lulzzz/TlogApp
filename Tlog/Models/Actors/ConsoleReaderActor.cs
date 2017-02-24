using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Akka.Actor;
using Tlog.Models.Files;

namespace Tlog.Models.Actors
{
    class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";

        #region Actors

        private IActorRef _consoleWriterActor;
        private IActorRef _tlogCoordinatorActor;

        #endregion

        public ConsoleReaderActor(
            IActorRef consoleWriterActor,
            IActorRef tlogCoordinatorActor)
        {
            _consoleWriterActor = consoleWriterActor;
            _tlogCoordinatorActor = tlogCoordinatorActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if(!String.IsNullOrEmpty(msg) && msg == StartCommand)
            {
                printInstructions();
            }

            getInput();
        }

        private void printInstructions()
        {
            Console.WriteLine("1.-Convertir Tlog");
        }

        private void getInput()
        {
            string input = Console.ReadLine();

            if (!String.IsNullOrEmpty(input) &&
                String.Equals(input, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }else if(input == "1")
            {
                doConvertTlog();
            }
        }

        private void doConvertTlog()
        {
            TlogFileModel tlogFileModel = new TlogFileModel();
            //get param
            Console.WriteLine("Ruta tlog:");

            tlogFileModel.BinFile = Console.ReadLine();

            tlogFileModel.TextFile = String.Format("{0}\\{1}.txt", 
                Path.GetDirectoryName(tlogFileModel.BinFile), 
                Path.GetFileNameWithoutExtension(tlogFileModel.BinFile));

            _tlogCoordinatorActor.Tell(new TlogCoordinatorActor.StartConversion(tlogFileModel, _consoleWriterActor));
        }
    }
}
