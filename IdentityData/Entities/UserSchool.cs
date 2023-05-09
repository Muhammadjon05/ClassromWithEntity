using IdentityData.Enums;

namespace IdentityData.Entities;

public class UserSchool
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid SchoolId { get; set; }
    public School School { get; set; }
    public UserStatus Status { get; set; }
}