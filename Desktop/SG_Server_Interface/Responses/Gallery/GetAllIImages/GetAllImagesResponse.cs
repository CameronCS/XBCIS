using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.GetAllIImages {
    public class GetAllImagesResponse {
        public List<GalleryImage> Images {
            get; set;
        }

        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetAllImagesResponse(List<GalleryImage> images, string message, int code) {
            this.Images = images;
            this.Message = message;
            this.Code = code;
        }

        internal GetAllImagesResponse(GetAllImagesResponseRaw raw, int code) {
            this.Images = raw.images;
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
