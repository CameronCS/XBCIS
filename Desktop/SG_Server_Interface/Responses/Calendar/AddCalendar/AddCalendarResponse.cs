using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Calendar.AddCalendar {
    public class AddCalendarResponse {
        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public AddCalendarResponse(string Message, int code) {
            this.Message = Message;
            this.Code = code;
        }

        internal AddCalendarResponse(AddCalendarResponseRaw raw, int code) {
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
