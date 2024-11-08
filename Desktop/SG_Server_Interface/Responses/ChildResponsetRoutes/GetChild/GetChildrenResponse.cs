using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.ChildResponsetRoutes.GetChild
{
    public class GetChildrenResponse
    {
        public int Code
        {
            get; set;
        }

        public List<Child> Children
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public GetChildrenResponse(int code, List<Child> children, string message)
        {
            Code = code;
            Children = children;
            Message = message;
        }

        public GetChildrenResponse(GetChildrenRaw raw, int code)
        {
            Code = code;
            Children = raw.Children;
            Message = raw.Message;
        }
    }
}
