using SG_Server_Interface.Classes;
using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.Resources.AddResouce;
using SG_Server_Interface.Responses.Resources.GetResources;
using SG_Server_Interface.Responses.Resources.GetResourcesMedia;
using SG_Server_Interface.Responses.UserRouteResponses.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net.RawHandlers {
    public class ResourceHandlerRaw {
        private static readonly HttpClient client = new();

        public static async Task<AddResourceResponse> AddResource(string url, Dictionary<string, string> body, string file_or_text, bool isFile) {
            AddResourceResponse @return;

            // Case for non-file (text resource)
            if (!isFile) {
                body.Add("tip_text", file_or_text);
                FormUrlEncodedContent content = new(body);

                HttpResponseMessage res = await client.PostAsync(url, content)
                    ?? throw new BadResponseExcpetion("HTTP response came back null! Check URLs before requesting.");

                AddResourceResponseRaw raw = await res.Content.ReadFromJsonAsync<AddResourceResponseRaw>()
                    ?? throw new BadResponseExcpetion("HTTP response came back null! Check URLs before requesting.");

                HttpStatusCode code = res.StatusCode;
                @return = new(raw, (int)code);
                return @return;
            } else {
                // Case for file upload
                using (MultipartFormDataContent form = new MultipartFormDataContent()) {
                    // Add form data (key-value pairs) to the form
                    foreach (var kvp in body) {
                        form.Add(new StringContent(kvp.Value), kvp.Key);
                    }

                    // Read the file into byte array
                    byte[] fileData = File.ReadAllBytes(file_or_text);
                    ByteArrayContent fileContent = new ByteArrayContent(fileData);

                    // Determine file extension and set the appropriate MIME type
                    string fileExtension = Path.GetExtension(file_or_text).ToLowerInvariant();
                    string mimeType = fileExtension switch {
                        ".pdf" => "application/pdf",
                        ".mp3" => "audio/mpeg",
                        ".mp4" => "video/mp4",
                        ".jpeg" => "image/jpeg",
                        ".jpg" => "image/jpeg",
                        ".png" => "image/png",
                        _ => "application/octet-stream"  // Default for unknown types
                    };

                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

                    // Add file to the form with a name and file name
                    form.Add(fileContent, "file", Path.GetFileName(file_or_text));

                    // Send the POST request
                    HttpResponseMessage res = await client.PostAsync(url, form)
                        ?? throw new BadResponseExcpetion("HTTP response came back null! Check URLs before requesting.");

                    // Deserialize the response body
                    AddResourceResponseRaw raw = await res.Content.ReadFromJsonAsync<AddResourceResponseRaw>()
                        ?? throw new BadResponseExcpetion("HTTP response came back null! Check URLs before requesting.");

                    HttpStatusCode code = res.StatusCode;
                    @return = new(raw, (int)code);

                    return @return;
                }
            }
        }


        public static async Task<GetFileResouceResponse> GetFileResouces(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            GetFileResourceResponseRaw raw = await res.Content.ReadFromJsonAsync<GetFileResourceResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;
            GetFileResouceResponse @return = new(raw, (int)code);
            return @return;
        }
        
        public static async Task<GetTipResouceResponse> GetTipResouces(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            GetTipResouceResponseRaw raw = await res.Content.ReadFromJsonAsync<GetTipResouceResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back null! Check URLS before requesting");

            HttpStatusCode code = res.StatusCode;
            GetTipResouceResponse @return = new(raw, (int)code);
            return @return;
        }
    }
}
