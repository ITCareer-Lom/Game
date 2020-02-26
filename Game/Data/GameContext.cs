namespace Game.Data
{
    using Microsoft.EntityFrameworkCore;

    public class GameContext : DbContext
    {
        public GameContext()  { }

        public GameContext(DbContextOptions options)
            : base(options)   { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
    }
}