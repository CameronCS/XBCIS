using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.UsernameAvaliable {
    public class CheckUsernameResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public CheckUsernameResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal CheckUsernameResponse(CheckUsernameResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
