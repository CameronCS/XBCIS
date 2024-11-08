using SG_Server_Interface.Classes;
using SG_Server_Interface.Responses.Resources.GetResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.GetResourcesMedia {
    public class GetFileResouceResponse {
        public List<FileResource> Resources {
            get; set;
        }

        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetFileResouceResponse(List<FileResource> resources, string message, int code) {
            this.Resources = resources;
            this.Message = message;
            this.Code = code;
        }

        internal GetFileResouceResponse(GetFileResourceResponseRaw raw, int code) {
            this.Resources = raw.Resources;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
