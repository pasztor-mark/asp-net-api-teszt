using System.ComponentModel.DataAnnotations;

namespace api_teszt.Models;

public class User
{
    [Key]
    public Guid id { get; set; } = Guid.NewGuid();
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string bio { get; set; }
    public List<Note> notes { get; set; } = new();
    
}