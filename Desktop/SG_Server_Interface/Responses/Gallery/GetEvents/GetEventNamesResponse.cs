using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.GetEvents {
    public class GetEventNamesResponse {
        public List<string> EventNames {
            get; set;
        }

        public List<string> ImagePaths {
            get; set;
        }

        public string Message {
            get; set;   
        }

        public int Code {
            get; set;
        }

        public GetEventNamesResponse(List<string> eventNames, List<string> imagePaths, string message, int code) {
            this.EventNames = eventNames;
            this.ImagePaths = imagePaths;
            this.Message = message;
            this.Code = code;
        }

        internal GetEventNamesResponse(GetEventNamesResponseRaw raw, int code) {
            this.EventNames = raw.events;
            this.ImagePaths = raw.image_paths;
            this.Message = raw.Message;
            this.Code = code;
        }
    }
}
