using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.GetParentContact
{
    public class ParentContactResponse
    {
        public List<ParentContact> Parents
        {
            get; set;
        }

        public int Code
        {
            get; set;
        }

        public ParentContactResponse(List<ParentContact> parents, int code)
        {
            Parents = parents;
            Code = code;
        }

        internal ParentContactResponse(ParentContactResponseRaw raw, int code)
        {
            Parents = raw.parents;
            Code = code;
        }
    }
}
