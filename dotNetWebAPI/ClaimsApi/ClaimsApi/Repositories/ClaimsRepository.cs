using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClaimsApi.Repositories
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly ApplicationDbContext _context;

        public ClaimsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync()
        {
            return await _context.Claims.ToListAsync();
        }

        public async Task<PagedResult<Claim>> GetClaimsPagedAsync(int page, int pageSize, string? searchTerm = null, string? status = null, string? sortBy = null, string? sortDirection = "asc")
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var query = _context.Claims.AsQueryable();

            // Apply search filter (as provided by user)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(c => 
                    (c.ClaimNumber != null && c.ClaimNumber.ToLower().Contains(lowerSearch)) || 
                    c.MemberName.ToLower().Contains(lowerSearch) || 
                    c.ProviderName.ToLower().Contains(lowerSearch));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(c => c.Status == status);
            }

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDesc = sortDirection?.ToLower() == "desc";
                switch (sortBy.ToLower())
                {
                    case "claimnumber":
                        query = isDesc ? query.OrderByDescending(c => c.ClaimNumber) : query.OrderBy(c => c.ClaimNumber);
                        break;
                    case "membername":
                        query = isDesc ? query.OrderByDescending(c => c.MemberName) : query.OrderBy(c => c.MemberName);
                        break;
                    case "providername":
                        query = isDesc ? query.OrderByDescending(c => c.ProviderName) : query.OrderBy(c => c.ProviderName);
                        break;
                    case "amount":
                        query = isDesc ? query.OrderByDescending(c => c.Amount) : query.OrderBy(c => c.Amount);
                        break;
                    case "status":
                        query = isDesc ? query.OrderByDescending(c => c.Status) : query.OrderBy(c => c.Status);
                        break;
                    default:
                        query = query.OrderByDescending(c => c.CreatedAt);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedAt);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Claim>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Items = items
            };
        }

        public async Task<Claim?> GetClaimByIdAsync(Guid id)
        {
            return await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Claim> AddClaimAsync(Claim claim)
        {
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task<bool> DeleteClaimAsync(Guid id)
        {
            var claim = await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
                return false;

            if (claim.Notes != null && claim.Notes.Any())
            {
                _context.ClaimNotes.RemoveRange(claim.Notes);
            }

            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ClaimNote>> GetNotesByClaimIdAsync(Guid claimId)
        {
            return await _context.ClaimNotes
                .Where(n => n.ClaimId == claimId)
                .ToListAsync();
        }

        public async Task<Claim?> UpdateClaimAsync(Claim claim)
        {
            // For complex updates like notes, we might need more logic
            // But if 'claim' is already tracked or we use Update, EF will handle most parts.
            
            // To handle Delete/Update of notes correctly in a generic UpdateClaimAsync:
            var existingClaim = await _context.Claims
                .Include(c => c.Notes)
                .FirstOrDefaultAsync(c => c.Id == claim.Id);
                
            if (existingClaim == null) return null;
            
            // Update simple properties
            _context.Entry(existingClaim).CurrentValues.SetValues(claim);
            
            // Update notes
            if (claim.Notes != null)
            {
                // Remove notes not in the incoming list
                var incomingIds = claim.Notes.Where(n => n.Id != Guid.Empty).Select(n => n.Id).ToHashSet();
                var toRemove = existingClaim.Notes?
                    .Where(en => !incomingIds.Contains(en.Id))
                    .ToList();
                
                if (toRemove != null && toRemove.Any())
                {
                    _context.ClaimNotes.RemoveRange(toRemove);
                }
                
                // Add or update notes
                foreach (var note in claim.Notes)
                {
                    var existingNote = existingClaim.Notes?.FirstOrDefault(en => en.Id == note.Id);
                    if (existingNote == null)
                    {
                        existingClaim.Notes?.Add(note);
                    }
                    else
                    {
                        _context.Entry(existingNote).CurrentValues.SetValues(note);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return existingClaim;
        }
    }
}
