namespace Full_Identity.DTOs;

public class CreateTaskDto
{
    public string? Title { get; set; }
    public ushort MaxBall { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; }
    
}
