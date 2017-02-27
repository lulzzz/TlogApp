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
    public class SalesFileModel
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

namespace Tlog.Models.Files.Comparers
{
    public class SalesFileComparer : IEqualityComparer<SalesFileModel>
    {
        private const string DATE_FORMAT_STR = "yyMMdd";

        public bool Equals(SalesFileModel x, SalesFileModel y)
        {
            return (
                x.Date.ToString(DATE_FORMAT_STR).Equals(y.Date.ToString(DATE_FORMAT_STR)) &&
                x.RegNum == y.RegNum &&
                x.CashierNum == y.CashierNum &&
                x.TxnNum == y.TxnNum
                );
        }

        public int GetHashCode(SalesFileModel obj)
        {
            string hashCode = String.Format("{0}{1}",
                obj.RegNum,
                obj.TxnNum);
            return Convert.ToInt32(hashCode);
        }
    }
}
