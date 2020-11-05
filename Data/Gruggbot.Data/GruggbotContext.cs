using Gruggbot.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Gruggbot.Data
{
    public class GruggbotContext : DbContext
    {

        public DbSet<Command> Commands { get; set; }

        public GruggbotContext(DbContextOptions options)
            :base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var connString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Gruggbot";
            optionsBuilder.UseSqlServer(connString);
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
