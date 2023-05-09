using IdentityData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityData.Congfigurations;

public class UserScienceConfiguration : IEntityTypeConfiguration<UserScience>
{
    public void Configure(EntityTypeBuilder<UserScience> builder)
    {
        builder.ToTable("user_science");
        builder.HasKey(science => new { science.UserId, science.ScienceId });
        
        builder.HasOne(science => science.User)
            .WithMany(science => science.UserSciences)
            .HasForeignKey(science => science.UserId);
        builder.HasOne(science => science.Science)
            .WithMany(science => science.UserSciences)
            .HasForeignKey(science => science.ScienceId);
    }
}