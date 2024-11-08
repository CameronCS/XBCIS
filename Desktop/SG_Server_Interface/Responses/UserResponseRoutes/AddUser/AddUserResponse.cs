using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.AddUser {
    public class AddUserResponse {
        public string Message {
            get; set;
        }
        
        public int Code {
            get; set; 
        }

        public AddUserResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddUserResponse(AddUserResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
