using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.SearchAll {
    internal class SearchAllUsernamesResponseRaw {
        public List<string> Usernames {
            get; set;
        }
        public string Message {
            get; set;
        }

        public SearchAllUsernamesResponseRaw(List<string> usernames, string message) {
            this.Usernames = usernames;
            this.Message = message;
        }
    }
}
