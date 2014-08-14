using System.Data.Entity;
using iConnect.Data.Configurations;
using iConnect.Data.Model;

namespace iConnect.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext():base("ChatConnection"){}
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
