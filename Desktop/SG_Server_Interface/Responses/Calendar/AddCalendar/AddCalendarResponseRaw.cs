using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Calendar.AddCalendar {
    internal class AddCalendarResponseRaw {
        public string message {
            get; set;
        }

        public AddCalendarResponseRaw(string message) {
            this.message = message;
        }
    }
}
