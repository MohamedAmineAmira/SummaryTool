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
        public DbSet<Message> Messages { get; set; }

    }

}
