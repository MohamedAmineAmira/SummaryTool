using Gateway.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Data
{
    public class TextDbContext : DbContext
    {
        public TextDbContext(DbContextOptions<TextDbContext> options) : base(options)
        {

        }

        public DbSet<Text> Texts { get; set; } = null!;
        public DbSet<DataPreprocessor> DataPreprocessors { get; set; }
        public DbSet<TextAnalyticsToolbox> TextAnalyticsToolboxes { get; set; }
        public DbSet<TextProcessor> TextProcessors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
