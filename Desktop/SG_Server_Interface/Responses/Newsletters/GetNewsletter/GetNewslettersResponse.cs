using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Newsletters.GetNewsletter {
    public class GetNewslettersResponse {
        public List<Newsletter> newsletters {
            get; set;
        }

        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetNewslettersResponse(List<Newsletter> newsletters, string message, int code) {
            this.newsletters = newsletters;
            this.Message = message;
            this.Code = code;
        }

        internal GetNewslettersResponse(GetNewsletterResponseRaw raw, int code) {
            this.newsletters = raw.results;
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
