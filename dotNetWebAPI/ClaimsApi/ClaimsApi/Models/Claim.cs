using System.ComponentModel.DataAnnotations;

public class Claim
{
    public Guid Id { get; set; }

    public string? ClaimNumber { get; set; }

    [Required]
    public string MemberName { get; set; } = null!;

    [Required]
    public string ProviderName { get; set; } = null!;

    [Range(1, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime ServiceDate { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<ClaimNote>? Notes { get; set; } = new List<ClaimNote>();
}