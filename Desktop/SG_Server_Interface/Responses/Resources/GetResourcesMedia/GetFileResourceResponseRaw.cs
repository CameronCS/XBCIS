using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Resources.GetResources {
    internal  class GetFileResourceResponseRaw {
        [JsonPropertyName("resources")]
        public List<FileResource> Resources {
            get; set;
        }
        [JsonPropertyName("message")]
        public string Message {
            get; set;
        }

        [JsonConstructor]
        internal GetFileResourceResponseRaw(List<FileResource> resources, string message) {
            this.Resources = resources;
            this.Message = message;
        }
    }
}
