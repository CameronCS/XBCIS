using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Calendar.GetCalendar {
    internal class GetCalendarEventResponseRaw {
        public List<CalendarEvent> results {
            get; set;
        }

        public GetCalendarEventResponseRaw(List<CalendarEvent> results) {
            this.results = results;
        }
    }
}
