using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.SearchSpecific {
    public class SearchSpecificResponse {
        public List<string> Usernames {
            get; set;
        }
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }


        public SearchSpecificResponse(List<string> usernames, string message, int code) {
            this.Usernames = usernames;
            this.Message = message;
            this.Code = code;
        }

        internal SearchSpecificResponse(SearchSpecificResponseRaw raw, int code) {
            this.Usernames = raw.Usernames;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
