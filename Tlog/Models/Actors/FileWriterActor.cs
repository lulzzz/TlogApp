using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Akka.Actor;
using Tlog.Models.Files;
using Tlog.Models.Messages;
namespace Tlog.Models.Actors
{
    class FileWriterActor : UntypedActor
    {
        #region Messages type
        
        public class StartWriter
        {
            /// <summary>
            /// Ruta del archivo a escribir
            /// </summary>
            public string FilePath { get; private set; }

            /// <summary>
            /// Datos a escribir
            /// </summary>
            public IEnumerable<object> Data { get; private set; }

            /// <summary>
            /// Log
            /// </summary>
            public IActorRef LogActor { get; private set; }

            public StartWriter(string filePath, IEnumerable<object> data, IActorRef logActor)
            {
                FilePath = filePath;
                Data = data;
                LogActor = logActor;
            }
        }
        
        #endregion

        protected override void OnReceive(object message)
        {
            if(message is StartWriter)
            {
                var msg = message as StartWriter;

                if(msg.Data is IEnumerable<SalesFileModel>)
                {
                    var data = msg.Data as IEnumerable<SalesFileModel>;
                    string[] lines = data.ToList().ConvertAll(
                        i => String.Format("{0},{1},{2},{3},{4},{5}",
                        i.Date.ToString("yyyyMMdd"),
                        i.RegNum.ToString(),
                        i.CashierNum.ToString(),
                        i.TxnNum.ToString(),
                        i.Total.ToString(),
                        i.FiscalPrinter
                        )).ToArray();
                    
                    File.WriteAllLines(msg.FilePath, lines);
                } 
            }
        }
    }
}
