using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.EmailsResponseRoute.SendEmail
{
    public class SendEmailResponse
    {
        public int Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public SendEmailResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public SendEmailResponse(SendEmailRaw raw, int code)
        {
            Message = raw.Message;
            Code = code;
        }
    }
}
