using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Tlog.Models.Actors;
using Tlog.Models.Logger;

namespace Tlog
{
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            MyActorSystem = ActorSystem.Create("MyActorSystem");

            IActorRef consoleLogActor = MyActorSystem.ActorOf(Props.Create(() => new LoggerActor(new ConsoleLog())), "consoleLogActor");
            IActorRef tloglCoordinatorActor = MyActorSystem.ActorOf(Props.Create(() => new TlogCoordinatorActor()), "tailCoordinator");
            IActorRef consoleReaderActor = MyActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleLogActor, tloglCoordinatorActor)), "consoleReaderActor");

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
