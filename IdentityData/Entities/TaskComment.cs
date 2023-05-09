namespace IdentityData.Entities;

public class TaskComment
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public TaskEntity Task { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string Message { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}