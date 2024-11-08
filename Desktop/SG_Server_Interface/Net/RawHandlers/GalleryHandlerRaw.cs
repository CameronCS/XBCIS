using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.Gallery.AddEvents;
using SG_Server_Interface.Responses.Gallery.GetAllIImages;
using SG_Server_Interface.Responses.Gallery.GetEvents;
using SG_Server_Interface.Responses.Gallery.GetYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net.RawHandlers {
    internal class GalleryHandlerRaw {
        private static HttpClient client = new();

        public static async Task<AddEventResponse> AddEvent(string url, Dictionary<string, string> req_body, string image_path) {
            MultipartFormDataContent form = new();
            // Add form data (key-value pairs) to the form
            foreach (var kvp in req_body) {
                form.Add(new StringContent(kvp.Value), kvp.Key);
            }

            byte[] imageData = File.ReadAllBytes(image_path);
            ByteArrayContent image = new(imageData);
            image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Adjust based on image type

            // Add image to the form with a name and file name
            form.Add(image, "image", Path.GetFileName(image_path));

            // Send the POST request
            HttpResponseMessage res = await client.PostAsync(url, form)
            ?? throw new BadResponseExcpetion("HTTP Response Came Back NULL");

            // Deserialize the response body
            AddEventResponseRaw? raw = await res.Content.ReadFromJsonAsync<AddEventResponseRaw>()
            ?? throw new BadResponseExcpetion("HTTP response came back null");

            // Get status code and return the response object
            HttpStatusCode code = res.StatusCode;
            AddEventResponse @return = new(raw, (int)code);
            return @return;
        }

        public static async Task<GetEventNamesResponse> GetEventNames(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            GetEventNamesResponseRaw? raw = await res.Content.ReadFromJsonAsync<GetEventNamesResponseRaw>()
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            HttpStatusCode code = res.StatusCode;
            GetEventNamesResponse @return = new(raw, (int)code);
            return @return;
        }

        public static async Task<GetAllImagesResponse> GetAllImages(string url, Dictionary<string, string> req_body) {
            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PostAsync(url, data)
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            GetAllImagesResponseRaw raw = await res.Content.ReadFromJsonAsync<GetAllImagesResponseRaw>()
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            HttpStatusCode code = res.StatusCode;
            GetAllImagesResponse @return = new(raw, (int)code);
            return @return;
        }

        public static async Task<GetYearResponse> GetEventYears(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            GetYearResponseRaw? raw = await res.Content.ReadFromJsonAsync<GetYearResponseRaw>()
            ??
            throw new BadResponseExcpetion("Response Came back null check url before posting");

            HttpStatusCode code = res.StatusCode;
            GetYearResponse @return = new(raw, (int)code);
            return @return;
        }
    }
}
