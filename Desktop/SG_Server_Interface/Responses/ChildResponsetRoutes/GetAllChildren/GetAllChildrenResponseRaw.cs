using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.GetAllChildren {
    public class GetAllChildrenResponseRaw {
        public List<Child> Children {
            get; set;
        }

        public string Message {
            get; set;
        }

        public GetAllChildrenResponseRaw(List<Child> children, string message) {
            this.Children = children;
            this.Message = message;
        }
    }
}
