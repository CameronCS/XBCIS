using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.AddResouce {
    internal class AddResourceResponseRaw {
        public string Message {
            get; set;
        }

        public AddResourceResponseRaw(string message) {
            this.Message = message;
        }
    }
}
