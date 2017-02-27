using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tlog.Models.Files
{
    /// <summary>
    /// Archivo tlog 
    /// </summary>
    class TlogFileModel
    {
        /// <summary>
        /// Archivo binario del tlog
        /// </summary>
        public string BinFile { get; set; }

        /// <summary>
        /// Archivo en formato texto del tlog
        /// </summary>
        public string TextFile { get; set; }
    }

    /// <summary>
    /// Archivo de Ventas
    /// </summary>
    class SalesFileModel
    {
        /// <summary>
        /// Caja
        /// </summary>
        public int RegNum { get; set; }

        /// <summary>
        /// Cajero
        /// </summary>
        public int CashierNum { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Numero de operacion
        /// </summary>
        public int TxnNum { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Impresora fiscal
        /// </summary>
        public string FiscalPrinter { get; set; }
    }
}
