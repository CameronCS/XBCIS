using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class CalendarEvent {
        public int id {
            get; set;
        }

        public string title {
            get; set;
        }

        public string description {
            get; set;
        }

        public DateTime event_date {
            get; set;
        }

        public DateTime created_at {
            get; set;
        }
    }
}
