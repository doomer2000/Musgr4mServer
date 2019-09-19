using System.Data.Entity;
using MServer.Models;
using MServer.Models.TestChat;

namespace MServer.EF
{
    public class Wheel_Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<Friend> Friends { get; set; }

        public DbSet<UnreadMessage> UnreadMessages { get; set; }
        //public DbSet<GeneralChatUsers> GeneralChatUsers { get; set; }

        //Chat
        public DbSet<ChatMember> ChatMembers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

       

        public Wheel_Context() : base("name=Wooden_Wheel_ConString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<Wheel_Context>(new DropCreateDatabaseIfModelChanges<Wheel_Context>());
            //Database.Create();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
