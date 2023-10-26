using Gateway.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Data
{
    public class TextDbContext : IdentityDbContext<ApplicationUser>
    {
        public TextDbContext(DbContextOptions<TextDbContext> options) : base(options)
        {

        }

        public DbSet<Text> Texts { get; set; } = null!;
        public DbSet<DataPreprocessor> DataPreprocessors { get; set; }
        public DbSet<TextAnalyticsToolbox> TextAnalyticsToolboxes { get; set; }
        public DbSet<TextProcessor> TextProcessors { get; set; }
        public DbSet<Log> Logs { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TextAnalyticsToolbox>()
                .HasMany(toolbox => toolbox.TextProcessors)
                .WithOne(summarizerModule => summarizerModule.Toolbox)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TextProcessor>()
                .HasOne(textProcessor => textProcessor.Toolbox)
                .WithMany(toolbox => toolbox.TextProcessors)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Text>()
                .HasOne(t => t.User)
                .WithMany(u => u.Texts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Texts)
                .WithOne(t => t.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Text>()
               .HasMany(text => text.Logs)
               .WithOne(log => log.Text)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Log>()
                .HasOne(log => log.Text)
                .WithMany(text => text.Logs)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TextProcessor>()
                .HasIndex(p => p.Name)
                .IsUnique();
            modelBuilder.Entity<DataPreprocessor>()
                .HasIndex(p => p.Name)
                .IsUnique();

        }
    }
}
