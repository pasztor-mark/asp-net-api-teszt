using api_teszt.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api_teszt.Services;

public interface INotesService
{
    Task<Note> GetNote(Guid id);
    Task<IEnumerable<Note>> GetNotesByUserEmail(string email);
    Task<Note> CreateNote(CreateNoteDto dto);
    Task<Note> UpdateNote(UpdateNoteDto dto, Guid id);
    Task<bool> DeleteNote(Guid id);
}

public class NotesService : INotesService
{
    protected readonly DatabaseContext _db;

    public NotesService(DatabaseContext db)
    {
        _db = db;
    }

    
    public async Task<Note> GetNote(Guid id)
    {
        return await _db.notes.FindAsync(id);
    }
    public async Task<IEnumerable<Note>> GetNotesByUserEmail(string email)
    {
        var author = await _db.users.Where(u => u.email == email).FirstOrDefaultAsync();
        if (author == null) return null;
        return await _db.notes.Where(n => n.author.id == author.id).ToListAsync();
    }
    public async Task<Note> CreateNote(CreateNoteDto dto)
    {
        var author = _db.users.FirstOrDefault(u => u.id == dto.user_id);
        if (author is null) return null;
        var created = new Note
        {
            author = author,
            note_content = dto.note_content,
            author_id = dto.user_id,
            tags = dto.tags.ToList()
        };
        var result = await _db.notes.AddAsync(created);
        await _db.SaveChangesAsync();
        return result.Entity;
    }
        
    public async Task<Note> UpdateNote(UpdateNoteDto dto, Guid id)
    {
        var found = await _db.notes.Where(n => n.id == id).FirstOrDefaultAsync();
        if (found is null) return null;

        if (!dto.tags.IsNullOrEmpty())
        {
            found.tags = dto.tags.ToList();
        }

        if (!dto.note_content.IsNullOrEmpty())
        {
            found.note_content = dto.note_content;
        }

        var upd = _db.Update(found);
        await _db.SaveChangesAsync();
        return upd.Entity;
    }

    public async Task<bool> DeleteNote(Guid id)
    {
        var found = await _db.notes.FindAsync(id);
        if (found is null) return false;
        _db.notes.Remove(found);
        await _db.SaveChangesAsync();
        return true;
    }
}