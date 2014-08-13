using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using iConnect.Server.Framework.Data.Model;

namespace iConnect.Server.Framework.Data
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(u => u.UserId);
            Property(u => u.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(u => u.FirstName).IsRequired();
            Property(u => u.LastName).IsRequired();
            Property(u => u.EmailAddress).IsRequired();
            Property(u => u.MiddleName).IsOptional();
            Property(u => u.Password).IsRequired();
            Property(u => u.Alias).IsRequired();
            HasMany(u => u.Messages).WithRequired(msg => msg.User).HasForeignKey(msg => msg.FromUserId);
        }
    }
}