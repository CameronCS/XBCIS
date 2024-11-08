using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.ForgotPassword {
    public class ForgotPasswordResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public ForgotPasswordResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal ForgotPasswordResponse(ForgotPasswordResponseRaw raw, int code) {
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
