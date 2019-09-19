using MServer.Models.TestChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MServer.Repositoies.Interfaces
{
    public interface ICommunicationService
    {
        void WriteUnreadMessage(Message message);
    }
}
