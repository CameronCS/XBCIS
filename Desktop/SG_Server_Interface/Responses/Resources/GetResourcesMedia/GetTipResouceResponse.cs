using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.GetResourcesMedia {
    public class GetTipResouceResponse {
        public List<TipResouce> Resouces {
            get; set;
        }

        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetTipResouceResponse(List<TipResouce> resouces, string message, int code) {
            this.Resouces = resouces;
            this.Message = message;
            this.Code = code;
        }

        internal GetTipResouceResponse(GetTipResouceResponseRaw raw, int code) {
            this.Resouces = raw.Resources;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
