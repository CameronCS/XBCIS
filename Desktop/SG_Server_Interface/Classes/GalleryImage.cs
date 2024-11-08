using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class GalleryImage {
        public int id {
            get; set;
        }

        public string image_path {
            get; set;
        }

        public GalleryImage(int id, string image_path) {
            this.id = id;
            this.image_path = image_path;
        }
    }
}
