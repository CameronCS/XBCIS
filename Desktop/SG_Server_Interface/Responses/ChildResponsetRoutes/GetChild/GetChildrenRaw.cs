using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.GetChild
{
    public class GetChildrenRaw
    {
        public List<Child> Children
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public GetChildrenRaw(List<Child> children, string message)
        {
            Children = children;
            Message = message;
        }
    }
}
