using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClaimsApi.Repositories;

namespace ClaimsApi.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IClaimsRepository _repository;

        public ClaimsService(IClaimsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _repository.GetClaimsAsync();
        }

        public async Task<PagedResult<Claim>> GetClaimsPagedAsync(int page, int pageSize, string? searchTerm = null, string? status = null, string? sortBy = null, string? sortDirection = "asc")
        {
            return await _repository.GetClaimsPagedAsync(page, pageSize, searchTerm, status, sortBy, sortDirection);
        }

        public async Task<Claim?> GetClaimByIdAsync(Guid id)
        {
            return await _repository.GetClaimByIdAsync(id);
        }

        public async Task<Claim> CreateClaimAsync(Claim claim)
        {
            // Business logic for new claims
            claim.Id = Guid.NewGuid();
            claim.ClaimNumber = "CLM-" + DateTime.Now.Ticks.ToString().Substring(10);
            claim.CreatedAt = DateTime.UtcNow;
            claim.UpdatedAt = DateTime.UtcNow;
            claim.Status = "Draft";

            return await _repository.AddClaimAsync(claim);
        }

        public async Task<bool> DeleteClaimAsync(Guid id)
        {
            return await _repository.DeleteClaimAsync(id);
        }

        public async Task<IEnumerable<ClaimNote>> GetNotesByClaimIdAsync(Guid claimId)
        {
            return await _repository.GetNotesByClaimIdAsync(claimId);
        }

        public async Task<Claim?> UpdateNotesAsync(Guid claimId, IEnumerable<ClaimNote> notes)
        {
            var claim = await _repository.GetClaimByIdAsync(claimId);
            if (claim == null)
                return null;

            var existingNotes = claim.Notes?.ToList() ?? new List<ClaimNote>();
            var incomingNotesList = notes.ToList();

            // Business logic for notes update
            var incomingIds = incomingNotesList.Where(n => n.Id != Guid.Empty).Select(n => n.Id).ToHashSet();
            
            // Notes to remove - handled by property assignment in repository if using EF tracking,
            // but for simplicity we'll keep the logic here and pass the updated collection.
            
            // This logic is actually a bit complex for a service if it depends on EF Change Tracker.
            // Let's keep the core orchestration here.
            
            // Prepare the updated notes collection
            var updatedNotes = new List<ClaimNote>();
            
            foreach (var note in incomingNotesList)
            {
                if (note.Id == Guid.Empty || !existingNotes.Any(en => en.Id == note.Id))
                {
                    // New note
                    note.Id = Guid.NewGuid();
                    note.ClaimId = claimId;
                    note.CreatedAt = DateTime.UtcNow;
                    updatedNotes.Add(note);
                }
                else
                {
                    // Update existing note
                    var existing = existingNotes.First(en => en.Id == note.Id);
                    existing.Note = note.Note;
                    updatedNotes.Add(existing);
                }
            }
            
            claim.Notes = updatedNotes;
            claim.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateClaimAsync(claim);
        }

        public async Task<Claim?> UpdateStatusAsync(Guid id, string status)
        {
            var claim = await _repository.GetClaimByIdAsync(id);
            if (claim == null)
                return null;

            claim.Status = status;
            claim.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateClaimAsync(claim);
        }
    }
}
