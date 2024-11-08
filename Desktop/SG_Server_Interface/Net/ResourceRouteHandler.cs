using SG_Server_Interface.Net.RawHandlers;
using SG_Server_Interface.Request;
using SG_Server_Interface.Responses.Resources.AddResouce;
using SG_Server_Interface.Responses.Resources.GetResourcesMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Net {
    public class ResourceRouteHandler(string API_URL, string Route) {
        private string API_URL = API_URL;
        private string Route = Route;

        public async Task<AddResourceResponse> AddResource(string title, string type, string admin_username, string file_path_or_text, bool isFile) {
            Dictionary<string, string> data = RequestHandler.Objectify(
                ["title", "uploaded_by", "type"],
                [title, admin_username, type]
            );

            string url = $"{this.API_URL}{this.Route}/add";
            AddResourceResponse @return = await ResourceHandlerRaw.AddResource(url, data, file_path_or_text, isFile);
            return @return;
        }

        public async Task<GetFileResouceResponse> GetFileResource(string resouce_type) {
            string url = $"{this.API_URL}{this.Route}/get?type={resouce_type}";
            GetFileResouceResponse @return = await ResourceHandlerRaw.GetFileResouces(url);
            return @return;
        }

        public async Task<GetTipResouceResponse> GetTipResource() {
            string url = $"{this.API_URL}{this.Route}/get?type=tips";
            GetTipResouceResponse @return = await ResourceHandlerRaw.GetTipResouces(url);
            return @return;
        }

        
    }
}
