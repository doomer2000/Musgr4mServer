using MServer.EF;
using MServer.Models;
using MServer.Models.TestChat;
using MServer.Repositoies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Repositoies
{
    public class CommunicationService : ICommunicationService
    {

        private Wheel_Context context;

        public CommunicationService(Wheel_Context context)
        {
            this.context = context;
        }

        public void WriteUnreadMessage(Message message)
        {
            context.UnreadMessages.Add(new UnreadMessage() { Chat = message.Chat, Message = message, UserTW = message.User });
        }
    }
}
