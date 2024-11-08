using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserRouteResponses.Login {
    internal class LoginRaw {
        public string Message {
            get; set;
        }

        public User User {
            get; set;
        }

        public LoginRaw(string message, User user) {
            Message = message;
            User = user;
        }
    }
}