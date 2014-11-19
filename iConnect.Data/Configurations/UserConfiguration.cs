using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using iConnect.Data.Model;

namespace iConnect.Data.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.UserId);
            Property(u => u.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);            
            HasMany(u => u.Messages).WithRequired(msg => msg.User).HasForeignKey(msg => msg.FromUserId);
        }
    }
}