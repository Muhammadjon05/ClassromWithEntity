using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityData.Entities;

public class School
{
    [Key]
    public Guid Id { get; set; }
    [Column("names")]
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? PhotoUrl { get; set; }
    
    public List<UserSchool> UserSchools { get; set; }
    public List<Science>? Sciences { get; set; }
}