using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<ClaimNote> ClaimNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>()
            .HasIndex(x => x.ClaimNumber)
            .IsUnique();

        modelBuilder.Entity<Claim>().HasData(

          new Claim
          {
              Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
              ClaimNumber = "CLM1001",
              MemberName = "Ravi Kumar",
              ProviderName = "Apollo Hospital",
              Amount = 15000,
              ServiceDate = new DateTime(2025, 1, 10),
              Status = "Submitted",
              CreatedAt = new DateTime(2025, 1, 10),
              UpdatedAt = new DateTime(2025, 1, 10)
          },

          new Claim
          {
              Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
              ClaimNumber = "CLM1002",
              MemberName = "Priya Sharma",
              ProviderName = "Fortis Hospital",
              Amount = 23000,
              ServiceDate = new DateTime(2025, 2, 5),
              Status = "Under Review",
              CreatedAt = new DateTime(2025, 2, 5),
              UpdatedAt = new DateTime(2025, 2, 5)
          },

          new Claim
          {
              Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
              ClaimNumber = "CLM1003",
              MemberName = "Arun Singh",
              ProviderName = "AIIMS",
              Amount = 5000,
              ServiceDate = new DateTime(2025, 3, 1),
              Status = "Approved",
              CreatedAt = new DateTime(2025, 3, 1),
              UpdatedAt = new DateTime(2025, 3, 1)
          },

          new Claim
          {
              Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
              ClaimNumber = "CLM1004",
              MemberName = "Meena Patel",
              ProviderName = "Max Healthcare",
              Amount = 12000,
              ServiceDate = new DateTime(2025, 2, 18),
              Status = "Rejected",
              CreatedAt = new DateTime(2025, 2, 18),
              UpdatedAt = new DateTime(2025, 2, 18)
          },

          new Claim
          {
              Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
              ClaimNumber = "CLM1005",
              MemberName = "Suresh Reddy",
              ProviderName = "Narayana Hospital",
              Amount = 17500,
              ServiceDate = new DateTime(2025, 3, 12),
              Status = "Submitted",
              CreatedAt = new DateTime(2025, 3, 12),
              UpdatedAt = new DateTime(2025, 3, 12)
          },

          new Claim
          {
              Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
              ClaimNumber = "CLM1006",
              MemberName = "Anita Verma",
              ProviderName = "Care Hospital",
              Amount = 8900,
              ServiceDate = new DateTime(2025, 1, 28),
              Status = "Approved",
              CreatedAt = new DateTime(2025, 1, 28),
              UpdatedAt = new DateTime(2025, 1, 28)
          },

          new Claim
          {
              Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
              ClaimNumber = "CLM1007",
              MemberName = "Rahul Nair",
              ProviderName = "Manipal Hospital",
              Amount = 21000,
              ServiceDate = new DateTime(2025, 4, 3),
              Status = "Under Review",
              CreatedAt = new DateTime(2025, 4, 3),
              UpdatedAt = new DateTime(2025, 4, 3)
          },

          new Claim
          {
              Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
              ClaimNumber = "CLM1008",
              MemberName = "Kavita Joshi",
              ProviderName = "Medanta Hospital",
              Amount = 9500,
              ServiceDate = new DateTime(2025, 3, 22),
              Status = "Submitted",
              CreatedAt = new DateTime(2025, 3, 22),
              UpdatedAt = new DateTime(2025, 3, 22)
          },

          new Claim
          {
              Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
              ClaimNumber = "CLM1009",
              MemberName = "Deepak Gupta",
              ProviderName = "Global Hospital",
              Amount = 30000,
              ServiceDate = new DateTime(2025, 4, 1),
              Status = "Approved",
              CreatedAt = new DateTime(2025, 4, 1),
              UpdatedAt = new DateTime(2025, 4, 1)
          },

          new Claim
          {
              Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
              ClaimNumber = "CLM1010",
              MemberName = "Lakshmi Narayan",
              ProviderName = "Sunrise Hospital",
              Amount = 14000,
              ServiceDate = new DateTime(2025, 4, 10),
              Status = "Under Review",
              CreatedAt = new DateTime(2025, 4, 10),
              UpdatedAt = new DateTime(2025, 4, 10)
          }
        );
    }
}