using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.AddEvents {
    public class AddEventResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddEventResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddEventResponse(AddEventResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
