using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.DeleteUser {
    internal class DeleteUserResponseRaw {
        public string Message {
            get; set;
        }

        public DeleteUserResponseRaw(string message) {
            this.Message = message;
        }
    }
}
