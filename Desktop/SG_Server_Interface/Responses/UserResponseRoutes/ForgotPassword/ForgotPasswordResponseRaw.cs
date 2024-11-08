using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.ForgotPassword {
    internal class ForgotPasswordResponseRaw {
        public string message {
            get; set;
        }

        public ForgotPasswordResponseRaw(string message) {
            this.message = message;
        }
    }
}
