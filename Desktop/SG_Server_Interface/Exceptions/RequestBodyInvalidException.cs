using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Exceptions {
    public class RequestBodyInvalidException(string msg) : Exception(msg) {
    }
}
