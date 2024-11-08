using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.Newsletters.AddNewsletter;
using SG_Server_Interface.Responses.Newsletters.GetNewsletter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net {
    public class NewsletterRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;

        public async Task<GetNewslettersResponse> GetNewsletters() {
            string url = $"{this.API_URL}{this.Route}/get";
            GetNewslettersResponse @return = await NewsletterHandlerRaw.GetNewsletters(url);
            return @return;
        }

        public async Task<AddNewsletterResponse> AddNewsletter(string title, string description, string image_path) {
            Dictionary<string, string> body = RequestHandler.Objectify(
                ["title", "description"], 
                [title, description]
            );
            string url = $"{this.API_URL}{this.Route}/add";
            AddNewsletterResponse @return = await NewsletterHandlerRaw.AddNewsletter(url, body, image_path);
            return @return;
        }
    }
}
