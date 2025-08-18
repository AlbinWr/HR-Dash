using HR_Dash.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR_Dash.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Modeller för databasen
        public DbSet<Anstalld> Anstallda { get; set; } = null!;
        public DbSet<Ansokan> Ansokningar { get; set; } = null!;
        public DbSet<Mote> Moten { get; set; }
        public DbSet<MoteAnstalld> MoteAnstallda { get; set; }
        public DbSet<Skift> Skift { get; set; } = null!;
        public DbSet<RullandeSchema> RullandeScheman { get; set; }
        public DbSet<AnstalldSchema> AnstalldSchema { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mote → Manager (ApplicationUser)
            modelBuilder.Entity<Mote>()
                .HasOne(m => m.Manager)
                .WithMany()
                .HasForeignKey(m => m.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // MoteAnstalld join
            modelBuilder.Entity<MoteAnstalld>()
                .HasKey(ma => new { ma.MoteId, ma.AnstalldId });

            // AnstalldSchema → Anstalld
            modelBuilder.Entity<AnstalldSchema>()
                .HasOne(x => x.Anstalld)
                .WithMany()
                .HasForeignKey(x => x.AnstalldId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnstalldSchema → RullandeSchema
            modelBuilder.Entity<AnstalldSchema>()
                .HasOne(x => x.RullandeSchema)
                .WithMany(x => x.Anstallda)
                .HasForeignKey(x => x.RullandeSchemaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Skift>()
                .HasOne(s => s.AnstalldSchema)
                .WithMany()
                .HasForeignKey(s => s.AnstalldSchemaId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
