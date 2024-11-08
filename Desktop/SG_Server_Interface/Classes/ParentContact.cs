using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class ParentContact {
        public string first_name {
            get; set;
        }

        public string last_name {
            get; set;
        }

        public string contact_no {
            get; set;
        }

        public ParentContact(string first_name, string last_name, string contact_no) {
            this.first_name = first_name;
            this.last_name = last_name;
            this.contact_no = contact_no;
        }
    }
}
