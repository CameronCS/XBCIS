using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class Newsletter {
        public string title {
            get; set;
        }

        public string description {
            get; set; 
        }

        public string image_path {
            get; set;
        }

        public Newsletter(string title, string description, string image_path) {
            this.title = title;
            this.description = description;
            this.image_path = image_path;
        }
    }
}
