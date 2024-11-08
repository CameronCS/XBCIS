using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.Newsletters.AddNewsletter;
using SG_Server_Interface.Responses.Newsletters.GetNewsletter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net.RawHandlers {
    public class NewsletterHandlerRaw {
        private static HttpClient client = new();

        public static async Task<GetNewslettersResponse> GetNewsletters(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("Response Came back null please check urls");

            GetNewsletterResponseRaw raw = await res.Content.ReadFromJsonAsync<GetNewsletterResponseRaw>()
            ??
            throw new BadResponseExcpetion("Response Came back null please check urls");

            HttpStatusCode code = res.StatusCode;
            GetNewslettersResponse @return = new(raw, (int)code);
            return @return;
        }

        public static async Task<AddNewsletterResponse> AddNewsletter(string url, Dictionary<string, string> req_body, string image_path) {
            using MultipartFormDataContent form = [];

            foreach (var kvp in req_body) {
                form.Add(new StringContent(kvp.Value), kvp.Key);
            }

            byte[] imageData = File.ReadAllBytes(image_path);
            ByteArrayContent image = new ByteArrayContent(imageData);
            string[] split = image_path.Split('.');
            image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{split[^1]}");

            form.Add(image, "image", Path.GetFileName(image_path));

            HttpResponseMessage res = await client.PostAsync(url, form)
            ?? throw new BadResponseExcpetion("HTTP Response Came Back NULL");

            AddNewsletterResponseRaw? raw = await res.Content.ReadFromJsonAsync<AddNewsletterResponseRaw>()
            ?? throw new BadResponseExcpetion("HTTP response came back null");

            HttpStatusCode code = res.StatusCode;
            AddNewsletterResponse @return = new(raw, (int)code);
            return @return;
        }
    }
}
