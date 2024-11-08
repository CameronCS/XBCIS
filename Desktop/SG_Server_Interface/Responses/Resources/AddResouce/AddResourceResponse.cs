using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.AddResouce {
    public class AddResourceResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddResourceResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddResourceResponse(AddResourceResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
