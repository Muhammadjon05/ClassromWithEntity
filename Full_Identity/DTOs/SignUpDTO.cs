namespace Full_Identity.DTOs;

public class SignUpDTO
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public IFormFile? Photo { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}