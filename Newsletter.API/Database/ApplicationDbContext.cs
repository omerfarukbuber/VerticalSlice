using Microsoft.EntityFrameworkCore;
using Newsletter.API.Entities;

namespace Newsletter.API.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(builder =>
            builder.OwnsOne(a => a.Tags, tagsBuilder => tagsBuilder.ToJson()));
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Article> Articles { get; set; }
}