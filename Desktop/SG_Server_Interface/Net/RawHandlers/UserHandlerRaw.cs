using System.Net;
using System.Net.Http.Json;
using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.UserRouteResponses.Login;
using SG_Server_Interface.Responses.UserResponseRoutes.SearchAll;
using SG_Server_Interface.Responses.UserResponseRoutes.AddUser;
using SG_Server_Interface.Responses.UserResponseRoutes.UsernameAvaliable;
using SG_Server_Interface.Responses.UserResponseRoutes.SearchSpecific;
using SG_Server_Interface.Responses.UserResponseRoutes.ResetPassword;
using SG_Server_Interface.Responses.UserResponseRoutes.DeleteUser;
using SG_Server_Interface.Responses.UserResponseRoutes.GetParentContact;
using SG_Server_Interface.Responses.UserResponseRoutes.ForgotPassword;

namespace SG_Server_Interface.Net.RawHandlers
{
    public class UserHandlerRaw {
        public static HttpClient client = new();

        public static async Task<AddUserResponse> AddUser(string url, Dictionary<string, string> req_body, string imagePath) {
            using (MultipartFormDataContent form = new MultipartFormDataContent()) {
                // Add form data (key-value pairs) to the form
                foreach (var kvp in req_body) {
                    form.Add(new StringContent(kvp.Value), kvp.Key);
                }

                // Read the image file into byte array
                byte[] imageData = File.ReadAllBytes(imagePath);
                ByteArrayContent image = new ByteArrayContent(imageData);
                string[] ext = imagePath.Split('.');
                image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"image/{ext[^1].ToUpper()}"); // Adjust based on image type

                // Add image to the form with a name, and file name
                form.Add(image, "image", Path.GetFileName(imagePath));

                // Send the POST request
                HttpResponseMessage res = await client.PostAsync(url, form)
                ?? throw new BadResponseExcpetion("HTTP Response Came Back NULL");

                // Deserialize the response body
                AddUserResponseRaw? raw = await res.Content.ReadFromJsonAsync<AddUserResponseRaw>()
                ?? throw new BadResponseExcpetion("HTTP response came back null");

                // Get status code and return the response object
                HttpStatusCode code = res.StatusCode;
                AddUserResponse @return = new(raw, (int)code);
                return @return;
            }
        }


        public static async Task<LoginResponse> Login(string url, Dictionary<string, string> req_body) {

            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PostAsync(url, data)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            LoginRaw? res_raw = await res.Content.ReadFromJsonAsync<LoginRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            LoginResponse @return = new(res_raw, (int)code);

            return @return;
        }

        public static async Task<CheckUsernameResponse> CheckUsernameAvaliable(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            CheckUsernameResponseRaw? res_raw = await res.Content.ReadFromJsonAsync<CheckUsernameResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            CheckUsernameResponse @return = new(res_raw, (int)code);
            return @return;
        }

        public static async Task<SearchAllUsernamesResponse> GetAllUsernames(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            SearchAllUsernamesResponseRaw res_raw = await res.Content.ReadFromJsonAsync<SearchAllUsernamesResponseRaw>()
            ?? 
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            SearchAllUsernamesResponse @return = new(res_raw, (int)code);
            return @return;
        }

        public static async Task<ParentContactResponse> GetParentContact(string url) {
            HttpResponseMessage res = await client.GetAsync(url) 
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            ParentContactResponseRaw raw = await res.Content.ReadFromJsonAsync<ParentContactResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;
            ParentContactResponse @return = new(raw, (int)code);
            return @return;
        }

        public static async Task<SearchSpecificResponse> SearchSpecificUsername(string url) {
            HttpResponseMessage res = await client.GetAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            SearchSpecificResponseRaw res_raw = await res.Content.ReadFromJsonAsync<SearchSpecificResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            SearchSpecificResponse @return = new(res_raw, (int)code);
            return @return;
        }

        public static async Task<ResetPasswordResponse> ResetPassword(string url, Dictionary<string, string> req_body) {
            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PutAsync(url, data)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            ResetPasswordResponseRaw? res_raw = await res.Content.ReadFromJsonAsync<ResetPasswordResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            ResetPasswordResponse @return = new(res_raw, (int)code);
            return @return;
        }

        public static async Task<ForgotPasswordResponse> ForgotPassword(string url, Dictionary<string, string> req_body) {
            FormUrlEncodedContent data = new(req_body);

            HttpResponseMessage res = await client.PutAsync(url, data)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            ForgotPasswordResponseRaw raw = await res.Content.ReadFromJsonAsync<ForgotPasswordResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode status = res.StatusCode;
            ForgotPasswordResponse @return = new(raw, (int)status);
            return @return;
        }

        public static async Task<DeleteUserResponse> DeleteUser(string url) {
            HttpResponseMessage res = await client.DeleteAsync(url)
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            DeleteUserResponseRaw? res_raw = await res.Content.ReadFromJsonAsync<DeleteUserResponseRaw>()
            ??
            throw new BadResponseExcpetion("HTTP response came back Null Check URLS before posting");

            HttpStatusCode code = res.StatusCode;

            DeleteUserResponse @return = new(res_raw, (int)code);
            return @return;
        }
    }
}
