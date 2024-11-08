namespace SG_Server_Interface.Responses.Gallery.GetEvents {
    internal class GetEventNamesResponseRaw {
        public List<string> events {
            get; set;
        }
        public List<string> image_paths {
            get; set;
        }

        public string Message {
            get; set;
        }

        public GetEventNamesResponseRaw(List<string> events, List<string> image_paths, string message) {
            this.events = events;
            this.image_paths = image_paths;
            this.Message = message;
        }
    }
}
