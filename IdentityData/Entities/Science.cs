namespace IdentityData.Entities;

public class Science
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public Guid SchoolId { get; set; }
    public School? School { get; set; }
    public List<UserScience>? UserSciences { get; set; }
    public List<TaskEntity> TaskEntities { get; set; }
}