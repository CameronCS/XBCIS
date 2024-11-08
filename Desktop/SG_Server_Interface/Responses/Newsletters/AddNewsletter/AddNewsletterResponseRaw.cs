using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Newsletters.AddNewsletter {
    internal class AddNewsletterResponseRaw {
        public string message {
            get; set;
        }

        public AddNewsletterResponseRaw(string message) {
            this.message = message;
        }
    }
}
