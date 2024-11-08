using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.UsernameAvaliable {
    internal class CheckUsernameResponseRaw {
        public string Message {
            get; set;
        }

        public CheckUsernameResponseRaw(string message) {
            this.Message = message;
        }
    }
}
