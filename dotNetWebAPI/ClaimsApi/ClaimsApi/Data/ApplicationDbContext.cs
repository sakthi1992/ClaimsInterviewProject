using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

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

        // Generate 5000 realistic-looking Claim seed records
        var rnd = new Random(0); // deterministic seed for reproducible data
        var statuses = new[] { "Submitted", "Under Review", "Approved", "Rejected" };
        var baseDate = new DateTime(2025, 1, 1);

        var firstNames = new[] { "Ravi", "Priya", "Arun", "Meena", "Suresh", "Anita", "Rahul", "Kavita", "Deepak", "Lakshmi", "Amit", "Sneha", "Vikram", "Neha", "Karan", "Pooja", "Manish", "Rekha", "Sandeep", "Divya", "Rohit", "Anjali", "Vijay", "Shreya", "Kunal", "Isha", "Ramesh", "Sunita", "Prakash", "Geeta" };
        var lastNames = new[] { "Kumar", "Sharma", "Singh", "Patel", "Reddy", "Verma", "Nair", "Joshi", "Gupta", "Narayan", "Chaudhary", "Mehta", "Kapoor", "Bose", "Khan", "Desai", "Malhotra", "Bhatia", "Rao", "Shetty", "Agarwal", "Iyer", "Menon", "Dubey", "Thomas", "Das", "Roy", "Prasad", "Saxena", "Nandan" };
        var providers = new[] {
            "Apollo Hospital", "Fortis Hospital", "AIIMS", "Max Healthcare", "Narayana Hospital", "Care Hospital", "Manipal Hospital", "Medanta Hospital", "Global Hospital", "Sunrise Hospital",
            "Aster Clinic", "KIMS Hospital", "Columbia Asia", "Wockhardt Hospitals", "Sir Ganga Ram Hospital", "Ruby Hall Clinic", "Kokilaben Dhirubhai Ambani Hospital", "Lilavati Hospital", "Sparsh Hospital", "Hindustan Hospital",
            "Yashoda Hospital", "Kauvery Hospital", "PD Hinduja Hospital", "BLK Super Speciality", "HCG Cancer Centre", "Bombay Hospital", "Cleveland Clinic India", "Carewell Hospital", "Narayana Multispeciality", "Sahyadri Hospital",
            "Hiranandani Hospital", "KJ Somaiya Hospital", "KEM Hospital", "Christian Medical College", "Breach Candy Hospital", "Jaslok Hospital", "Deenanath Mangeshkar Hospital", "Artemis Hospital", "BLK-Max Hospital", "Columbia Hospital"
        };

        var claims = Enumerable.Range(0, 5000).Select(i =>
        {
            var fn = firstNames[rnd.Next(firstNames.Length)];
            var ln = lastNames[rnd.Next(lastNames.Length)];
            var provider = providers[rnd.Next(providers.Length)];
            var daysOffset = i % 365;

            return new Claim
            {
                Id = Guid.NewGuid(),
                ClaimNumber = $"CLM-{1001 + i}",
                MemberName = $"{fn} {ln}",
                ProviderName = provider,
                Amount = rnd.Next(1000, 30001),
                ServiceDate = baseDate.AddDays(daysOffset),
                Status = statuses[i % statuses.Length],
                CreatedAt = baseDate.AddDays(daysOffset),
                UpdatedAt = baseDate.AddDays(daysOffset)
            };
        }).ToArray();

        modelBuilder.Entity<Claim>().HasData(claims);
    }
}