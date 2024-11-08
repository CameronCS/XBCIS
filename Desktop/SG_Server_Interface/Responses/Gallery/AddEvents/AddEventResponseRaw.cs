using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.AddEvents {
    internal class AddEventResponseRaw {
        public string Message {
            get; set;
        }
        public AddEventResponseRaw(string message) {
            this.Message = message;
        }
    }
}
