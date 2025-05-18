using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_teszt.Enums;
using api_teszt.Models;

public class Note
{
    [Key] public Guid id { get; set; } = Guid.NewGuid();
    public Guid author_id { get; set; }
    
    [ForeignKey(nameof(author_id))]
    public User author { get; set; }
    public string note_content { get; set; }
    public List<Tag> tags { get; set; } = new List<Tag>();
    
    
}