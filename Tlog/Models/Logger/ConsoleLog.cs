using System;

namespace Tlog.Models.Logger
{
    public class ConsoleLog : ILogger
    {
        /// <summary>
        /// Escribe el log por consola
        /// </summary>
        /// <param name="log">log</param>
        /// <param name="logType"><see cref="LogType"/></param>
        public void Log(string log, LogType logType)
        {
            setForegroundColor(logType);
            writeLine($"/{logType.ToString()} {DateTime.Now.ToString("hh:mm:ss")} : {log}");
            resetForegroundColor();
        }

        /// <summary>
        /// Fija el color <see cref="Console.ForegroundColor"/> dependiendo del <see cref="LogType"/>
        /// </summary>
        /// <param name="logType"><see cref="LogType"/> del log</param>
        private void setForegroundColor(LogType logType)
        {
            if (logType == LogType.Error)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (logType == LogType.Warning)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (logType == LogType.Success)
                Console.ForegroundColor = ConsoleColor.Green;
        }

        /// <summary>
        /// <see cref="Console.WriteLine(string)"/>
        /// </summary>
        /// <param name="line"></param>
        private void writeLine(string line)
        {
            Console.WriteLine();
            Console.WriteLine(line);
        }

        /// <summary>
        /// <see cref="Console.ResetColor"/>
        /// </summary>
        private void resetForegroundColor()
        {
            Console.ResetColor();
        }
    }
}
