using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.Gallery.AddEvents;
using SG_Server_Interface.Responses.Gallery.GetAllIImages;
using SG_Server_Interface.Responses.Gallery.GetEvents;
using SG_Server_Interface.Responses.Gallery.GetYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net {
    public class GalleryRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;

        public async Task<AddEventResponse> AddEvent(string event_name, string year, string image_path) {
            string url = $"{this.API_URL}{this.Route}/add";
            Dictionary<string, string> req_body = RequestHandler.Objectify(
                ["event_name", "year"], 
                [event_name, year]
            );
            AddEventResponse @return = await GalleryHandlerRaw.AddEvent(url, req_body, image_path);
            return @return;
        }
        public async Task<GetEventNamesResponse> GetEventNames(string year) {
            string url = $"{this.API_URL}{this.Route}/get-names?year={year}";
            GetEventNamesResponse @return = await GalleryHandlerRaw.GetEventNames(url);
            return @return;
        }

        public async Task<GetAllImagesResponse> GetAllImages(string year, string event_name) {
            string url = $"{this.API_URL}{this.Route}/get?year={year}";
            Dictionary<string, string> req_body = RequestHandler.Objectify(["event_name"], [event_name]);
            GetAllImagesResponse @return = await GalleryHandlerRaw.GetAllImages(url, req_body);
            return @return;
        }

        public async Task<GetYearResponse> GetYears() {
            string url = $"{this.API_URL}{this.Route}/get-years";
            GetYearResponse @return = await GalleryHandlerRaw.GetEventYears(url);
            return @return;
        }
    }
}
