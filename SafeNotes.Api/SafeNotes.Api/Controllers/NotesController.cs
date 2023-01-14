using AngleSharp.Io;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeNotes.Application.Models.Notes;
using SafeNotes.Application.Services;

namespace SafeNotes.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;

        public NotesController(NoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.CreateNote(request, cancellationToken);
            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.GetError());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> CreateNote(int id, [FromBody] UpdateAllowedUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.UpdateAllowedUsers(
                request with { NoteId = id }, cancellationToken);
            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(result.GetError());
        }
    }
}
