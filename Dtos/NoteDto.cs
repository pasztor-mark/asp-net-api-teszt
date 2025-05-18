using System.ComponentModel.DataAnnotations;
using api_teszt.Enums;

public class CreateNoteDto
{
    [Required]
    public string note_content { get; set; }
    [Required]
    public Guid user_id { get; set; }
    public HashSet<Tag> tags { get; set; } = new();
}

public class NoteDto
{
    public string note_content { get; set; }
    public HashSet<Tag> tags { get; set; } = new();

    public NoteDto(Note note)
    {
        this.tags = new HashSet<Tag>(note.tags);
        this.note_content = note.note_content;
    }
}

public class UpdateNoteDto
{
    #nullable enable
    public string? note_content { get; set; }
    public HashSet<Tag>? tags { get; set; }
}
