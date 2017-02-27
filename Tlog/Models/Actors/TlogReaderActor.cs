using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using Akka.Actor;
using Tlog.Models.Files;

namespace Tlog.Models.Actors
{
    /// <summary>
    /// Lectura de tlog convertido
    /// </summary>
    class TlogReaderActor : UntypedActor
    {
        #region Messages types

        /// <summary>
        /// Mensaje para leer datos de ventas
        /// </summary>
        public class Sales
        {
            /// <summary>
            /// Ruta del tlog convertido
            /// </summary>
            public string TlogFilePath { get; private set; }
            
            public Sales(string tlogFilePath)
            {
                TlogFilePath = tlogFilePath;
            }
        }

        #endregion

        private IActorRef _loggerActor;

        public TlogReaderActor(IActorRef logActor)
        {
            _loggerActor = logActor;
        }

        protected override void OnReceive(object message)
        {
            if(message is Sales)
            {
                var msg = message as Sales;

                List<SalesFileModel> data = getSales(readAllTlogLines(msg.TlogFilePath));

                data = data.Distinct().ToList();

                Context.ActorOf(Props.Create(() => new FileWriterActor()), "fileWriterActor")
                    .Tell(new FileWriterActor.StartWriter(
                        String.Format("{0}\\{1}.Sales.txt",
                            Path.GetDirectoryName(msg.TlogFilePath),
                            Path.GetFileNameWithoutExtension(msg.TlogFilePath)),
                        data,
                        _loggerActor
                        ));            
                
                _loggerActor.Tell(new Messages.Success(String.Format("{0} registros leidos", data.Count)));
            }
        }

        /// <summary>
        /// Lee todas las lineas del archivo tlog
        /// </summary>
        /// <param name="tlogFilePath"></param>
        /// <returns></returns>
        private string[] readAllTlogLines(string tlogFilePath)
        {
            if (File.Exists(tlogFilePath))
                return File.ReadAllLines(tlogFilePath);
            
            return null;
        }

        /// <summary>
        /// Extrae los datos de ventas del tlog
        /// </summary>
        /// <param name="tlog"></param>
        /// <returns></returns>
        private List<SalesFileModel> getSales(string[] tlog)
        {
            List<SalesFileModel> data = new List<SalesFileModel>();

            if (tlog != null && tlog.Length > 0)
            {
                foreach (string ua in
                    tlog.Where<string>(
                           l => l.Trim() != String.Empty
                        && l.Trim().Length > 38
                        && l.Trim().Substring(0, 2).Equals("UA")))
                {
                    foreach (string m in tlog
                     .Where<string>(
                            l => l.Trim() != String.Empty
                         && l.Trim().Length > 90
                         && l.Trim().Substring(33, 4).Equals(ua.Substring(33, 4))
                         && l.Trim().Substring(7, 2).Equals(ua.Substring(7, 2))
                         && l.Trim().Substring(9, 6).Equals(ua.Substring(9, 6))
                         && l.Trim().Substring(21, 6).Equals(ua.Substring(21, 6))
                         && l.Trim().Substring(0, 1).Equals("M")
                         && l.Trim().Substring(90, 2).Equals("FP")))
                    {
                        string t = tlog.Where<string>(l => l.Trim() != String.Empty
                                                        && l.Trim().Length > 56
                                                        && l.Trim().Substring(33, 4).Equals(ua.Substring(33, 4))
                                                        && l.Trim().Substring(7, 2).Equals(ua.Substring(7, 2))
                                                        && l.Trim().Substring(9, 6).Equals(ua.Substring(9, 6))
                                                        && l.Trim().Substring(21, 6).Equals(ua.Substring(21, 6))
                                                        && l.Trim().Substring(0, 1).Equals("T")).First();
                        decimal totalwotax = 0;

                        if (!String.IsNullOrEmpty(t))
                        {
                            totalwotax = Convert.ToDecimal(t.Substring(48, 6) + "." + t.Substring(54, 2));
                            if (t.Substring(56, 1) == "-")
                            {
                                totalwotax = totalwotax * -1;
                            }
                        }

                        data.Add(new SalesFileModel()
                        {
                            RegNum = Convert.ToInt16(ua.Substring(7, 2)),
                            CashierNum = Convert.ToInt32(ua.Substring(9, 6)),
                            TxnNum = Convert.ToInt16(ua.Substring(33, 4)),
                            Total = totalwotax,
                            Date = DateTime.ParseExact(ua.Substring(21, 6), "yyMMdd", CultureInfo.InvariantCulture),
                            FiscalPrinter = m.Substring(90, 20)
                        });
                            
                    }
                }
            }
            return data;
        }
    }
}
