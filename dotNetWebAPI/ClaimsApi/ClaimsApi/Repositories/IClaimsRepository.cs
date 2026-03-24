using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaimsApi.Repositories
{
    public interface IClaimsRepository
    {
        Task<IEnumerable<Claim>> GetClaimsAsync();
        Task<PagedResult<Claim>> GetClaimsPagedAsync(int page, int pageSize, string? searchTerm = null, string? status = null, string? sortBy = null, string? sortDirection = "asc");
        Task<Claim?> GetClaimByIdAsync(Guid id);
        Task<Claim> AddClaimAsync(Claim claim);
        Task<bool> DeleteClaimAsync(Guid id);
        Task<IEnumerable<ClaimNote>> GetNotesByClaimIdAsync(Guid claimId);
        Task<Claim?> UpdateClaimAsync(Claim claim);
    }
}
