using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.ResetPassword {
    internal class ResetPasswordResponseRaw {
        public string Message {
            get; set;
        }

        public ResetPasswordResponseRaw(string message) {
            this.Message = message;
        }
    }
}
