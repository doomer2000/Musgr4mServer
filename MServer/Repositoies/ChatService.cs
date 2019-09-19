using MServer.EF;
using MServer.Models;
using MServer.Models.TestChat;
using MServer.Repositoies.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Repositoies
{
    public class ChatService : IChatService
    {
        private Wheel_Context context;

        public ChatService(Wheel_Context context)
        {
            this.context = context;
        }

        public void UnreadMessage(User user,Message mes)
        {
            context.UnreadMessages.Add(new UnreadMessage()
            {
                Chat = mes.Chat,
                Message = mes,
                UserTW = user
            });
        }

        public async Task<Chat> GetChatById(int id)
        {
            return await context.Chats.FirstOrDefaultAsync(c => c.Id == id);
        }
        public Chat CreateChat(Chat chat)
        {
            context.Chats.Add(chat);
            context.SaveChanges();
            return chat;
        }
    }
}
