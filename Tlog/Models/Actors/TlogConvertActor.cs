using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Akka.Actor;

using Tlog.Models.Files;

namespace Tlog.Models.Actors
{
    class TlogConvertActor : UntypedActor
    {
        #region Message types
        /// <summary>
        /// Siguiente cvtlog para realizar la conversion
        /// </summary>
        public class Next
        {
            public string Cvtlog { get; private set; }

            public Next(string cvtlog)
            {
                Cvtlog = cvtlog;
            }
        }
        #endregion


        private string[] _cvtlogs;
        private int _indexOfCvtlog;
        private TlogFileModel _tlogFile;
        private IActorRef _reporterActor;

        public TlogConvertActor(TlogFileModel tlogFile, IActorRef reporterActor)
        {
            _tlogFile = tlogFile;
            _reporterActor = reporterActor;
            _cvtlogs = ConfigurationManager.AppSettings["Cvtlog"].Split(';');
            _indexOfCvtlog = 0;

            next();
        }

        protected override void OnReceive(object message)
        {
             if(message is Next)
            {
                var msg = message as Next;

                ProcessStartInfo processInfo = new ProcessStartInfo("cmd",
                    String.Format("/c " + msg.Cvtlog + " \"{0}\" \"{1}\"", _tlogFile.TextFile, _tlogFile.BinFile));
                processInfo.UseShellExecute = true;

                Process process = Process.Start(processInfo);
                process.WaitForExit(4000);
                process.Close();

                if (File.Exists(_tlogFile.TextFile))
                {
                    _reporterActor.Tell(new Messages.Success(String.Format("Archivo {0} convertido", _tlogFile.BinFile)));
                }
                else
                {
                    _indexOfCvtlog = _indexOfCvtlog + 1;

                    if (_indexOfCvtlog < _cvtlogs.Length)
                    {
                        next();
                    }
                    else
                    {
                        _reporterActor.Tell(new Messages.Error("El archivo no pudo ser convertido por los cvtlog actuales"));
                    }
                }              
            }  
        }

        private void next()
        {
            Self.Tell(new Next(_cvtlogs[_indexOfCvtlog]));
        }
    }
}
