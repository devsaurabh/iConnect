using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using iConnect.Server.Framework.Data.Model;

namespace iConnect.Server.Framework.Data
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            HasKey(m=> m.MessageId);
            Property(m => m.MessageId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
      
    }
}