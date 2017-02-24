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
}
