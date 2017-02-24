using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Tlog.Models.Actors
{
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if(message is Messages.Success)
            {
                var msg = message as Messages.Success;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msg.Reason);
            }else if(message is Messages.Error)
            {
                var msg = message as Messages.Error;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.Reason);
            }else
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();
        }
    }
}
