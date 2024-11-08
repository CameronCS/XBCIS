using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.SearchAll {
    public class SearchAllUsernamesResponse {
        public int Code {
            get; set;
        }

        public List<string> Usernames {
            get; set;
        }

        public string Message {
            get; set;
        }

        public SearchAllUsernamesResponse(int code, List<string> usernames, string message) {
            this.Code = code;
            this.Usernames = usernames;
            this.Message = message;
        }

        internal SearchAllUsernamesResponse(SearchAllUsernamesResponseRaw raw, int code) {
            this.Usernames = raw.Usernames;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
