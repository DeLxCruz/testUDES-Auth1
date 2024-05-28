using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(50);

            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50);

            builder.Property(p => p.TypeIdentification).IsRequired().HasMaxLength(50);

            builder.Property(p => p.DocumentNumber).IsRequired().HasMaxLength(15);

            builder.Property(p => p.Email).IsRequired().HasMaxLength(50);

            builder.Property(p => p.Username).IsRequired().HasMaxLength(50);

            builder.Property(p => p.Password).IsRequired().HasMaxLength(255);

            builder.HasMany(p => p.Roles).WithMany(p => p.Users).UsingEntity<UserRoles>(
                j => j.HasOne(p => p.Role).WithMany(p => p.UserRoles).HasForeignKey(p => p.RoleId),
                j => j.HasOne(p => p.User).WithMany(p => p.UserRoles).HasForeignKey(p => p.UserId),
                j => {j.ToTable("UserRoles"); j.HasKey(p => new {p.UserId, p.RoleId});}
            );
        }
    }
}