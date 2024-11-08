using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.GetAllIImages {
    internal class GetAllImagesResponseRaw {
        public List<GalleryImage> images {
            get; set;
        }
        public string message {
            get; set;
        }

        public GetAllImagesResponseRaw(List<GalleryImage> images, string message) {
            this.images = images;
            this.message = message;
        }
    }
}
