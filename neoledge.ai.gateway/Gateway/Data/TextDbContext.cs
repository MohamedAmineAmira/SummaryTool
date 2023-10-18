using Gateway.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Data
{
    public class TextDbContext : IdentityDbContext
    {
        public TextDbContext(DbContextOptions<TextDbContext> options) : base(options)
        {

        }

        public DbSet<Text> Texts { get; set; } = null!;
        public DbSet<DataPreprocessor> DataPreprocessors { get; set; }
        public DbSet<TextAnalyticsToolbox> TextAnalyticsToolboxes { get; set; }
        public DbSet<TextProcessor> TextProcessors { get; set; }
        public DbSet<User> Users { get; set; } = null!;
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
        }
    }
}
