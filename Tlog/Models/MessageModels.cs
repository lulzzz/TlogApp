using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tlog.Models.Messages
{
    
    class Error
    {
        public string Reason { get; set; }

        public Error(string reason) { Reason = reason; }
    }

    class Success
    {
        public string Reason { get; set; }

        public Success(string reason) { Reason = reason; }
    }
    
}
