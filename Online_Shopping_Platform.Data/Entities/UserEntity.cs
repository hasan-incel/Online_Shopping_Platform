using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }      
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }      
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Customer,
        Admin
    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(40);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(40);


            base.Configure(builder);
        }
    }
}
