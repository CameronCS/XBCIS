using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.AddChild {
    public class AddChildResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddChildResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddChildResponse(AddChildResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
