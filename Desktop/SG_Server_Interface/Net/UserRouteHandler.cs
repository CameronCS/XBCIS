using SG_Server_Interface.Exceptions;
using SG_Server_Interface.Responses.UserRouteResponses.Login;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SG_Server_Interface.Request;
using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Responses.UserResponseRoutes.SearchAll;
using SG_Server_Interface.Responses.UserResponseRoutes.AddUser;
using SG_Server_Interface.Classes;
using SG_Server_Interface.Responses.UserResponseRoutes.GetParentContact;
using SG_Server_Interface.Responses.UserResponseRoutes.ResetPassword;
using SG_Server_Interface.Responses.UserResponseRoutes.ForgotPassword;

namespace SG_Server_Interface.Net
{
    public class UserRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;

        public async Task<AddUserResponse> AddUser(string fname, string lname, string username, string password, string isadmin, string contact_no, string imagePath) {
            Dictionary<string, string> req_body = RequestHandler.Objectify(
                ["first_name", "last_name", "username", "password", "is_admin", "contact_no"], 
                [fname, lname, username, password, isadmin, contact_no]
            );
            string url = $"{this.API_URL}{this.Route}/add";
            AddUserResponse @return = await UserHandlerRaw.AddUser(url, req_body, imagePath);
            return @return;      
        }

        public async Task<LoginResponse> Login(string username, string password) {
            string url = $"{this.API_URL}{this.Route}/login";
            LoginResponse @return = await UserHandlerRaw.Login(url, RequestHandler.Objectify(["username", "password"], [username, password]));
            return @return;
        }

        public async Task<SearchAllUsernamesResponse> GetAllUsernames(string searching) {
            string url = $"{this.API_URL}{this.Route}/all-usernames?searching={searching}";
            SearchAllUsernamesResponse @return = await UserHandlerRaw.GetAllUsernames(url);
            return @return;
        }

        public async Task<ParentContactResponse> GetParentContact(int child_id) {
            string url = $"{this.API_URL}{this.Route}/get-contact?child_id={child_id}";
            ParentContactResponse @return = await UserHandlerRaw.GetParentContact(url);
            return @return;
        }

        public async Task<ResetPasswordResponse> ResetPassword(string username, string current_pass, string new_pass) {
            string url = $"{this.API_URL}{this.Route}/reset-password";
            Dictionary<string, string> body = RequestHandler.Objectify(
                ["username", "current_pass", "new_pass"], 
                [username, current_pass, new_pass]
            );
            ResetPasswordResponse @return = await UserHandlerRaw.ResetPassword(url, body);
            return @return;
        }

        public async Task<ForgotPasswordResponse> ForgotPassword(string username, string password) {
            string url = $"{this.API_URL}{this.Route}/forgot-password";
            Dictionary<string, string> body = RequestHandler.Objectify(
                ["username", "password"], 
                [username, password]
            );
            ForgotPasswordResponse @return = await UserHandlerRaw.ForgotPassword(url, body);
            return @return;
        }
    }
}
