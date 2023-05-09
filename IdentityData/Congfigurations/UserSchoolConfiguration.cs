using IdentityData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityData.Congfigurations;

public class UserSchoolConfiguration: IEntityTypeConfiguration<UserSchool>
{
    public void Configure(EntityTypeBuilder<UserSchool> builder)
    {
            builder.ToTable("user_school");
            builder.HasKey(school => new { school.UserId, school.SchoolId });
            builder.HasOne(school => school.User)
                .WithMany(school => school.UserSchools)
                .HasForeignKey(school => school.UserId);
            builder.HasOne(school => school.School).WithMany(school => school.UserSchools)
                .HasForeignKey(school => school.SchoolId);
    }
}