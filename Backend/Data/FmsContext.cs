using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using File = Backend.Models.File;

namespace Backend.Data
{
    public class FmsContext : IdentityDbContext<User>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Procedure> Procedures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("data source=Database.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<File>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Files)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Procedure>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.File)
                    .WithMany(f => f.Procedures)
                    .HasForeignKey(p => p.FileId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Procedures)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            List<IdentityRole> roles =
            [
                new IdentityRole
                {
                    Name = "admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "user",
                    NormalizedName = "USER"
                }
            ];
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry is { State: EntityState.Added, Entity: ITimestampable newTimestampable, })
                {
                    newTimestampable.CreatedOn = DateTime.Now;
                    newTimestampable.UpdatedOn = DateTime.Now;
                }
                else if (entry is { State: EntityState.Modified, Entity: ITimestampable editedTimestampable, })
                {
                    editedTimestampable.UpdatedOn = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
