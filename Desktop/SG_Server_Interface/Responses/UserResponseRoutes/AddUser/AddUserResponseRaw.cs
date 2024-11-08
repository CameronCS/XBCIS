using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.AddUser {
    internal class AddUserResponseRaw {
        public string Message {
            get; set;
        }

        public AddUserResponseRaw(string message) {
            this.Message = message;
        }
    }
}
