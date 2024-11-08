using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.GetYears {
    public class GetYearResponseRaw {
        public List<string> results {
            get; set;
        }  // List of strings, simple
        public string message {
            get; set;
        }

        public GetYearResponseRaw(List<string> results, string message) {
            this.results = results;
            this.message = message;
        }
    }

}
