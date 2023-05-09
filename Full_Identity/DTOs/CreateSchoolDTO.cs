namespace Full_Identity.DTOs;

public class CreateSchoolDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? Photo { get; set; }
}