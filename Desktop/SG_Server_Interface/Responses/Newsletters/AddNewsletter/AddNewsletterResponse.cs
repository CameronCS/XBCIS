using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Newsletters.AddNewsletter {
    public class AddNewsletterResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddNewsletterResponse(string message, int code) {
            this.Message = message;
            this.Code = code;
        }

        internal AddNewsletterResponse(AddNewsletterResponseRaw raw, int code) {
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
