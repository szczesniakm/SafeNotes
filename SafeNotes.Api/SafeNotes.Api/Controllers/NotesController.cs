using AngleSharp.Io;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeNotes.Application.Models;
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
        [ProducesResponseType(typeof(IdModel), StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.CreateNote(request, cancellationToken);
            if (result.Success)
            {
                return StatusCode(StatusCodes.Status201Created,result.GetOk());
            }

            return BadRequest(result.GetError());
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNote(int id, [FromBody] UpdateNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.UpdateNote(new UpdateNoteModel(id, request.Title, request.Content, request.Key), cancellationToken);
            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(result.GetError());
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(GetNoteListResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNoteList(CancellationToken cancellationToken)
        {
            var result = await _noteService.GetNoteList(cancellationToken);
            if (result.Success)
            {
                return Ok(result.GetOk());
            }

            return BadRequest(result.GetError());
        }

        [HttpGet("{id}/access")]
        [ProducesResponseType(typeof(GetAllowedUsersResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllowedUsers(int id, CancellationToken cancellationToken)
        {
            var result = await _noteService.GetAllowedUsers(id, cancellationToken);
            if (result.Success)
            {
                return Ok(result.GetOk());
            }

            return BadRequest(result.GetError());
        }

        [HttpPut("{id}/access")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateAllowedUsers(int id, [FromBody] UpdateAllowedUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.UpdateAllowedUsers(
                request with { NoteId = id }, cancellationToken);
            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest(result.GetError());
        }

        [HttpPut("get/{id}")]
        [ProducesResponseType(typeof(GetNoteResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetNote(int id, [FromBody] GetNoteRequest request, CancellationToken cancellationToken)
        {
            var result = await _noteService.GetNote(new GetNoteModel(id, request.Key), cancellationToken);
            if (result.Success)
            {
                return Ok(result.GetOk());
            }

            return BadRequest(result.GetError());
        }
    }
}
