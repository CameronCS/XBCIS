using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.GetAllChildren {
    public class GetAllChildrenResponse {
        public int Code {
            get; set;
        }

        public List<Child> Children {
            get; set; 
        }

        public string Message {
            get; set;
        }

        public GetAllChildrenResponse(int code, List<Child> children, string message) {
            this.Code = code;
            this.Children = children;
            this.Message = message;
        }

        public GetAllChildrenResponse(GetAllChildrenResponseRaw raw, int code) {
            this.Children = raw.Children;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
