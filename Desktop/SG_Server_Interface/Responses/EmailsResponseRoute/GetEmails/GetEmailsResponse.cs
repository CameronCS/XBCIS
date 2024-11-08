using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.EmailsResponseRoute.GetEmails
{
    public class GetEmailsResponse
    {
        public List<Emails> Emails
        {
            get; set;
        }

        public int Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public GetEmailsResponse(List<Emails> emails, int code, string message)
        {
            Emails = emails;
            Code = code;
            Message = message;
        }

        public GetEmailsResponse(GetEmailsRaw raw, int code)
        {
            Emails = raw.Emails;
            Message = raw.Message;
            Code = code;
        }
    }
}
