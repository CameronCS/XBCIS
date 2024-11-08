using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.EmailsResponseRoute.GetEmails;
using SG_Server_Interface.Responses.EmailsResponseRoute.SendEmail;
using SG_Server_Interface.Request;

namespace SG_Server_Interface.Net.RawHandlers {
    public class EmailHandlerRaw {
        private static readonly HttpClient client = new();
        public static async Task<GetEmailsResponse> GetEmailsByReceiver(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            GetEmailsRaw? res_raw = await res.Content.ReadFromJsonAsync<GetEmailsRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;

            GetEmailsResponse @return = new(res_raw, (int)code);

            return @return;
        }

        public static async Task<SendEmailResponse> SendEmail(string url, Dictionary<string, string> req_body) {
            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PostAsync(url, data)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            SendEmailRaw? res_raw = await res.Content.ReadFromJsonAsync<SendEmailRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            SendEmailResponse @return = new(res_raw, (int)code);

            return @return;
        }
    }
}
