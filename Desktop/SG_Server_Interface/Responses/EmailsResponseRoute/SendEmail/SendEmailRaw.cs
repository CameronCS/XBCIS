using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.EmailsResponseRoute.SendEmail
{
    public class SendEmailRaw
    {
        public string Message
        {
            get; set;
        }

        public SendEmailRaw(string message)
        {
            Message = message;
        }
    }
}
