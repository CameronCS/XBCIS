using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.Calendar.AddCalendar;
using SG_Server_Interface.Responses.Calendar.GetCalendar;

namespace SG_Server_Interface.Net {
    public class CalendarRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;
        public async Task<GetCalendarEventResponse> GetCalendarEvents() {
            string url = $"{this.API_URL}{this.Route}/get";
            GetCalendarEventResponse @return = await CalendarHandlerRaw.GetCalendarEvent(url);
            return @return;
        }

        public async Task<AddCalendarResponse> AddCalenderEvent(string title, string desc, string date) {
            Dictionary<string, string> body = RequestHandler.Objectify(
                ["title", "description", "event_date"], 
                [title, desc, date]
            );

            string url = $"{this.API_URL}{this.Route}/add";
            AddCalendarResponse @return = await CalendarHandlerRaw.AddCalendarEvent(url, body);
            return @return;
        }
    }
}
