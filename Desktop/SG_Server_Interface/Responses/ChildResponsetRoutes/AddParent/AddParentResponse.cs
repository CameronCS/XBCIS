using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.AddParent {
    public class AddParentResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddParentResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddParentResponse(AddParentResponseRaw raw, int code) {
            this.Message = Message;
            this.Code = code;
        }
    }
}
