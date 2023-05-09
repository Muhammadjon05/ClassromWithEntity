namespace IdentityData.Entities;

public class TaskEntity
{
    public string? Title { get; set; }
    public Guid Id { get; set; }
    public ushort MaxBall { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; } 
    
    public Guid ScienceId { get; set; }
    public Science Science { get; set; }
    
    public TaskStatus Status { get; set; }
    
    public List<UserTask> UserTasks { get; set; }
    
    public List<TaskComment> TaskComments { get; set; }
}