using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Newsletters.GetNewsletter {
    internal class GetNewsletterResponseRaw {
        public List<Newsletter> results {
            get; set;
        }
        
        public string message {
            get; set;
         }

        public GetNewsletterResponseRaw(List<Newsletter> results, string message) {
            this.results = results;
            this.message = message;
        }
    }
}
