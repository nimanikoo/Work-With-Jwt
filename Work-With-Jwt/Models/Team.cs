using System.ComponentModel.DataAnnotations;

namespace Authentication_with_Jwt.Models;

public class Team
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Country { get; set; }

    [MaxLength(50)]
    public string TeamPrinciple { get; set; }
}