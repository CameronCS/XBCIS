using SG_Server_Interface.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Request {
    internal class RequestHandler {
        public static Dictionary<string, string> Objectify(string[] args, object[] vals) {
            if (args.Length != vals.Length) {
                throw new RequestBodyInvalidException("Arguments and Values must be of the same length");
            }

            return args.Zip(vals, (arg, val) => new { arg, val = val?.ToString() ?? "null" }).ToDictionary(x => x.arg, x => x.val);
        }

    }
}
