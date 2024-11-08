using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.ChildResponsetRoutes.AddChild;
using SG_Server_Interface.Responses.ChildResponsetRoutes.AddParent;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetAllChildren;
using SG_Server_Interface.Responses.ChildResponsetRoutes.GetChild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net {
    public class ChildRouteHandler(string API_URL, string Route) {
        private readonly string API_URL = API_URL;
        private readonly string Route = Route;

        public async Task<AddChildResponse> AddChild(string fname, string lname, string cname, string notes, string image_path, string? p1, string? p2) {
            Dictionary<string, string> body = RequestHandler.Objectify(
                ["first_name", "last_name", "class_name", "notes"],
                [fname, lname, cname, notes]
            );

            AddChildResponse @return;

            if (p1 == null) {
                @return = await ChildHandlerRaw.AddChild($"{this.API_URL}{Route}/add", body, image_path);
            } else if (p2 == null) {
                @return = await ChildHandlerRaw.AddChild($"{this.API_URL}{Route}/add?parent_1={p1}", body, image_path);
            } else {
                @return = await ChildHandlerRaw.AddChild($"{this.API_URL}{Route}/add?parent1={p1}&parent2={p2}", body, image_path);
            }

            return @return;
        }

        public async Task<AddParentResponse> AddParent(string cfname, string clname, string pun) {
            Dictionary<string, string> req_body = RequestHandler.Objectify(
                ["child_f_name", "child_l_name", "parent_username"], 
                [cfname, clname, pun]
            );

            AddParentResponse @return = await ChildHandlerRaw.AddParent($"{this.API_URL}{this.Route}/add-parent", req_body);
            
            return @return;
        }

        public async Task<GetChildrenResponse> GetChild(string username) {
            string url = $"{this.API_URL}{Route}/get?username={username}";
            GetChildrenResponse @return = await ChildHandlerRaw.GetChild(url);
            return @return;
        }

        public async Task<GetAllChildrenResponse> GetAllChildren(string username) {
            string url = $"{this.API_URL}{this.Route}/get-all?username=${username}";
            GetAllChildrenResponse @return = await ChildHandlerRaw.GetAllChildren(url);
            return @return;
        }
    }
}
