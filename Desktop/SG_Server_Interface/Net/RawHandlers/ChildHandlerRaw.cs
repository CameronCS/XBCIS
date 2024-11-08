using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetChild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetAllChildren;
using SG_Server_Interface.Responses.ChildResponsetRoutes.AddChild;
using SG_Server_Interface.Responses.ChildResponsetRoutes.AddParent;

namespace SG_Server_Interface.Net.RawHandlers {
    public class ChildHandlerRaw {
        private static HttpClient client = new();

        public static async Task<AddChildResponse> AddChild(string url, Dictionary<string, string> req_body, string imagePath) {
            using (MultipartFormDataContent form = new MultipartFormDataContent()) {
                // Add form data (key-value pairs) to the form
                foreach (var kvp in req_body) {
                    form.Add(new StringContent(kvp.Value), kvp.Key);
                }

                // Read the image file into byte array
                byte[] imageData = File.ReadAllBytes(imagePath);
                ByteArrayContent image = new ByteArrayContent(imageData);
                image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Adjust based on image type

                // Add image to the form with a name and file name
                form.Add(image, "image", Path.GetFileName(imagePath));

                // Send the POST request
                HttpResponseMessage res = await client.PostAsync(url, form)
                ?? 
                throw new BadResponseExcpetion("HTTP Response Came Back NULL");

                // Deserialize the response body
                AddChildResponseRaw? raw = await res.Content.ReadFromJsonAsync<AddChildResponseRaw>()
                ?? throw new BadResponseExcpetion("HTTP response came back null");

                // Get status code and return the response object
                HttpStatusCode code = res.StatusCode;
                AddChildResponse @return = new(raw, (int)code);
                return @return;
            }
        }

        public static async Task<AddParentResponse> AddParent(string url, Dictionary<string, string> req_body) {
            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PostAsync(url, data)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            AddParentResponseRaw? res_raw = await res.Content.ReadFromJsonAsync<AddParentResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;

            AddParentResponse @return = new(res_raw, (int)code);

            return @return;
        }

        public static async Task<GetChildrenResponse> GetChild(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            GetChildrenRaw? res_raw = await res.Content.ReadFromJsonAsync<GetChildrenRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;

            GetChildrenResponse @return = new(res_raw, (int)code);

            return @return;
        }

        public static async Task<GetAllChildrenResponse> GetAllChildren(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            GetAllChildrenResponseRaw? res_raw = await res.Content.ReadFromJsonAsync<GetAllChildrenResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;

            GetAllChildrenResponse @return = new(res_raw, (int)code);

            return @return;
        }
    }
}
