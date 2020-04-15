using ChatBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ViberUserEntity> ViberUsers { get; set; }

        public DbSet<ViberUserMessageEntity> ViberUserMessages { get; set; }

        public DbSet<DialogflowResultEntity> DialogflowResults { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
