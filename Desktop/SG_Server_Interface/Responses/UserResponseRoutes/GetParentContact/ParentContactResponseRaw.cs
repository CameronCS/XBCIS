using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.UserResponseRoutes.GetParentContact
{
    internal class ParentContactResponseRaw
    {
        public List<ParentContact> parents
        {
            get; set;
        }
        // Added Comment
    }
}
