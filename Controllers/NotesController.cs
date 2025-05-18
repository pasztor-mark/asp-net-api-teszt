using api_teszt.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_teszt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        protected readonly INotesService _notesService;

        public NotesController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetNotesByUserEmail(string email)
        {
            var result = await _notesService.GetNotesByUserEmail(email);
            if (!result.Any()) return NotFound();
            return Ok(result.Select(n => new NoteDto(n)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(Guid id)
        {
            var result = await _notesService.GetNote(id);
            if (result is null) return NotFound();
            return Ok(new NoteDto(result));
        }

        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote(CreateNoteDto dto)
        {
            var result = await _notesService.CreateNote(dto);
            if (result is null) return BadRequest();
            Console.WriteLine(result.id);
            return CreatedAtAction(nameof(GetNotesByUserEmail), new { email = result.author.email }, new NoteDto(result));
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDto>> UpdateNote(UpdateNoteDto dto, Guid id)
        {
            var result = await _notesService.UpdateNote(dto, id);
            if (result is null) return NotFound();
            return Ok(new NoteDto(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteNote(Guid id)
        {
            var result = await _notesService.DeleteNote(id);
            if (result is false) return NotFound();
            return NoContent();
        }
    }
}
