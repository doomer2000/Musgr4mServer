using System.Data.Entity;
using MServer.Models;

namespace MServer.EF
{
    public class Wheel_Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<PrivChat> PrivChats { get; set; }
        public DbSet<ChatMsgs> ChatMsgs { get; set; }
        public DbSet<GeneralChat> GeneralChats { get; set; }
        //public DbSet<GeneralChatUsers> GeneralChatUsers { get; set; }

        public Wheel_Context() : base("name=Wooden_Wheel_ConString")
        {
            //Configuration.ProxyCreationEnabled = false;
            //Configuration.ValidateOnSaveEnabled = false;
            Database.SetInitializer<Wheel_Context>(new DropCreateDatabaseIfModelChanges<Wheel_Context>());
            //Database.Create();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
