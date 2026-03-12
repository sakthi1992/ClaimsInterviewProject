using System.Text.Json.Serialization;

public class ClaimNote
{
    public Guid Id { get; set; }

    public Guid ClaimId { get; set; }

    public string Note { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public Claim? Claim { get; set; }
}