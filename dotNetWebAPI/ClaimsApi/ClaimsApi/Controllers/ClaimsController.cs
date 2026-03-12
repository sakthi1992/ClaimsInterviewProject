using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClaimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims()
        {
            var claims = await _context.Claims.ToListAsync();
            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClaimById(Guid id)
        {
            var claim = await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return NotFound();

            return Ok(claim);
        }


        [HttpPost]
        public async Task<IActionResult> CreateClaim([FromBody] Claim claim)
        {
            claim.Id = Guid.NewGuid();
            claim.ClaimNumber = "CLM-" + DateTime.Now.Ticks.ToString().Substring(10);
            claim.CreatedAt = DateTime.UtcNow;
            claim.UpdatedAt = DateTime.UtcNow;
            claim.Status = "Draft";

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            var claim = await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return NotFound();

            // Remove any related notes first to avoid FK issues
            if (claim.Notes != null && claim.Notes.Any())
            {
                _context.ClaimNotes.RemoveRange(claim.Notes);
            }

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/notes")]
        public async Task<IActionResult> GetNotes(Guid id)
        {
            var notes = await _context.ClaimNotes
                .Where(n => n.ClaimId == id)
                .ToListAsync();

            return Ok(notes);
        }

        [HttpPut("{id}/notes")]
        public async Task<IActionResult> UpdateNotesClaims(Guid id, [FromBody] List<ClaimNote> notes)
        {
            var claim = await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return NotFound();

            var existingNotes = claim.Notes?.ToList() ?? new List<ClaimNote>();

            // Determine incoming note ids (skip empty ids for new notes)
            var incomingIds = notes.Where(n => n.Id != Guid.Empty).Select(n => n.Id).ToHashSet();

            // Remove notes that are not present in the incoming list
            var toRemove = existingNotes.Where(en => !incomingIds.Contains(en.Id)).ToList();
            if (toRemove.Any())
            {
                _context.ClaimNotes.RemoveRange(toRemove);
            }

            foreach (var note in notes)
            {
                if (note.Id == Guid.Empty || !existingNotes.Any(en => en.Id == note.Id))
                {
                    // New note
                    note.Id = Guid.NewGuid();
                    note.ClaimId = id;
                    note.CreatedAt = DateTime.UtcNow;
                    _context.ClaimNotes.Add(note);
                }
                else
                {
                    // Update existing note text
                    var existing = existingNotes.First(en => en.Id == note.Id);
                    existing.Note = note.Note;
                    _context.ClaimNotes.Update(existing);
                }
            }

            claim.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(claim);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateClaimStatus(Guid id, [FromBody] string status)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
                return NotFound();

            claim.Status = status;
            claim.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(claim);
        }
    }
}
