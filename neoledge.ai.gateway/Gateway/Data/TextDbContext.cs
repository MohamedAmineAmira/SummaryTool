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
        public DbSet<SummarizerModule> SummarizerModules { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TextAnalyticsToolbox>()
                .HasMany(toolbox => toolbox.SummarizerModules)
                .WithOne(summarizerModule => summarizerModule.Toolbox)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SummarizerModule>()
                .HasOne(summarizerModule => summarizerModule.Toolbox)
                .WithMany(toolbox => toolbox.SummarizerModules)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
