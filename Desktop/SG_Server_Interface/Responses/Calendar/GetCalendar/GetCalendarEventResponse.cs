using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Calendar.GetCalendar {
    public class GetCalendarEventResponse {
        public List<CalendarEvent> Events {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetCalendarEventResponse(List<CalendarEvent> events, int code) {
            this.Events = events;
            this.Code = code;
        }

        internal GetCalendarEventResponse(GetCalendarEventResponseRaw raw, int code) {
            this.Events = raw.results;
            this.Code = code;
        }
    }
}
