using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClaimsApi.Repositories;
using ClaimsApi.Services;

namespace ClaimsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsService _service;

        public ClaimsController(IClaimsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims()
        {
            var claims = await _service.GetClaimsAsync();
            return Ok(claims);
        }

        // Paginated claims endpoint
        [HttpGet("paged")]
        public async Task<IActionResult> GetClaimsPaged(int page = 1, int pageSize = 20, string? searchTerm = null, string? status = null, string? sortBy = null, string? sortDirection = "asc")
        {
            var result = await _service.GetClaimsPagedAsync(page, pageSize, searchTerm, status, sortBy, sortDirection);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaimById(Guid id)
        {
            var claim = await _service.GetClaimByIdAsync(id);

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }


        [HttpPost]
        public async Task<IActionResult> CreateClaim([FromBody] Claim claim)
        {
            var createdClaim = await _service.CreateClaimAsync(claim);
            return Ok(createdClaim);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            var success = await _service.DeleteClaimAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}/notes")]
        public async Task<IActionResult> GetNotes(Guid id)
        {
            var notes = await _service.GetNotesByClaimIdAsync(id);
            return Ok(notes);
        }

        [HttpPut("{id}/notes")]
        public async Task<IActionResult> UpdateNotesClaims(Guid id, [FromBody] List<ClaimNote> notes)
        {
            var claim = await _service.UpdateNotesAsync(id, notes);

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateClaimStatus(Guid id, [FromBody] string status)
        {
            var claim = await _service.UpdateStatusAsync(id, status);
            if (claim == null)
                return NotFound();

            return Ok(claim);
        }
    }
}
