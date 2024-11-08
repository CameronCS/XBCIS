using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.Gallery.GetYears {
    public class GetYearResponse {
        public List<string> Years {
            get; set;
        }

        public string Message {
            get; set;
        }

        public int Code {
            get; set;
        }

        public GetYearResponse(List<string> years, string message, int code) {
            this.Years = years;
            this.Message = message;
            this.Code = code;
        }

        internal GetYearResponse(GetYearResponseRaw raw, int code) {
            this.Years = raw.results;
            this.Message = raw.message;
            this.Code = code;
        }
    }
}
