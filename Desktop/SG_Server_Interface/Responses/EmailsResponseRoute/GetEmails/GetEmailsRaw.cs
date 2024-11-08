using SG_Server_Interface.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Responses.EmailsResponseRoute.GetEmails
{
    public class GetEmailsRaw
    {
        public List<Emails> Emails
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public GetEmailsRaw(List<Emails> emails, string message)
        {
            Emails = emails;
            Message = message;
        }

    }
}
