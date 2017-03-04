using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tlog.Models.Logger
{
    /// <summary>
    /// Tipo de log
    /// </summary>
    public enum LogType
    {
        Error,
        Warning,
        Success,
        Debug,
        Info
    }

    /// <summary>
    /// Permite el registro de logs
    /// </summary>
    interface ILogger
    {
        /// <summary>
        /// Escribe un log 
        /// </summary>
        /// <param name="log">mensaje</param>
        /// <param name="logType">tipo de log <see cref="LogType"/></param>
        void Log(string log, LogType logType);
    }
}
