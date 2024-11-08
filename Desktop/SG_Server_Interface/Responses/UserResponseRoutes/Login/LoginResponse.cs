using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserRouteResponses.Login {
    public class LoginResponse {
        public User User {
            get; set;
        }

        public int StatusCode {
            get; set;
        }

        public string Message {
            get; set;
        }

        public LoginResponse(User user, int statusCode, string message) {
            User = user;
            StatusCode = statusCode;
            Message = message;
        }

        internal LoginResponse(LoginRaw raw, int statusCode) {
            User = raw.User;
            Message = raw.Message;
            StatusCode = statusCode;
        }
    }
}