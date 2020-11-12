namespace Gruggbot.Data
{
    using System.Reflection;

    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;

    public class GruggbotContext : DbContext
    {
        public GruggbotContext(DbContextOptions<GruggbotContext> options)
            : base(options)
        {
        }

        public DbSet<Command> Commands { get; set; }

        public DbSet<CountdownCommand> CountdownCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Command>()
                .ToTable("Commands");

            modelBuilder.Entity<CountdownCommand>()
                .ToTable("CountdownCommands");

            modelBuilder.Entity<CommandAlias>()
                .ToTable("Aliases");

            modelBuilder.Entity<CountdownCommand>()
                .ToTable("CountdownCommands");
        }
    }
}
