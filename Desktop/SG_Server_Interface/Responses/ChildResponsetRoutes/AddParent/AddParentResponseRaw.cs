using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.AddParent {
    internal class AddParentResponseRaw {
        public string Message {
            get; set;
        }

        public AddParentResponseRaw(string message) {
            this.Message = message;
        }
    }
}
