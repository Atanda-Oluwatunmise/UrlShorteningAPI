using Microsoft.EntityFrameworkCore;
using UrlShortener.Entites;
using UrlShortener.Services;

namespace UrlShortener
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                builder.Property(s => s.UniqueCode).HasMaxLength(UrlShorteningService.Noofcharinshortlink);
                builder.HasIndex(s=> s.UniqueCode).IsUnique();
            });
        }
    }
}
