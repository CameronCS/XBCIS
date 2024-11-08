using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.GetResourcesMedia {
    internal class GetTipResouceResponseRaw {
        public List<TipResouce> Resources {
            get; set;
        }

        public string Message {
            get; set;
        }

        public GetTipResouceResponseRaw(List<TipResouce> resources, string message) {
            this.Resources = resources;
            this.Message = message;
        }
    }
}
