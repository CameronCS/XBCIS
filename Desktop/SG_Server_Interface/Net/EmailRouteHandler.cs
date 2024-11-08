using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.EmailsResponseRoute.GetEmails;
using SG_Server_Interface.Responses.EmailsResponseRoute.SendEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net {
    public class EmailRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;

        public async Task<GetEmailsResponse> GetEmailsByUsername(string username) {
            string url = $"{this.API_URL}{Route}/get?receiver_username={username}";
            GetEmailsResponse @return = await EmailHandlerRaw.GetEmailsByReceiver(url);
            return @return;
        }

        public async Task<SendEmailResponse> SendEmail(string sender_username, string receiver_username, string subject, string body) {
            Dictionary<string, string> req_body= RequestHandler.Objectify(
                ["sender_username", "receiver_username", "subject", "body"],
                [sender_username, receiver_username, subject, body]
            );

            string url = $"{this.API_URL}{this.Route}/send";
            SendEmailResponse @return = await EmailHandlerRaw.SendEmail(url, req_body);
            return @return;
        }
    }
}
