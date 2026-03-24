using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaimsApi.Services
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<PagedResult<Claim>> GetClaimsPagedAsync(int page, int pageSize, string? searchTerm = null, string? status = null, string? sortBy = null, string? sortDirection = "asc");
        Task<Claim?> GetClaimByIdAsync(Guid id);
        Task<Claim> CreateClaimAsync(Claim claim);
        Task<bool> DeleteClaimAsync(Guid id);
        Task<IEnumerable<ClaimNote>> GetNotesByClaimIdAsync(Guid claimId);
        Task<Claim?> UpdateNotesAsync(Guid claimId, IEnumerable<ClaimNote> notes);
        Task<Claim?> UpdateStatusAsync(Guid id, string status);
    }
}
