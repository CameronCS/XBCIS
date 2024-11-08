using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.DeleteUser {
    public class DeleteUserResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public DeleteUserResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal DeleteUserResponse(DeleteUserResponseRaw raw, int code) {
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
