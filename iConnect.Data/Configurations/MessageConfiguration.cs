using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using iConnect.Data.Model;

namespace iConnect.Data.Configurations
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