using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.SearchSpecific {
    internal class SearchSpecificResponseRaw {
        public List<string> Usernames {
            get; set;
        }

        public string Message {
            get; set;
        }

        public SearchSpecificResponseRaw(List<string> usernames,  string message) {
            this.Usernames = usernames;
            this.Message = message;
        }
    }
}
