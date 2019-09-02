using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Services
{
    public class MailService
    {
        SmtpClient smtpClient;

        public MailService()
        {
            smtpClient = new SmtpClient();
        }
    }
}
