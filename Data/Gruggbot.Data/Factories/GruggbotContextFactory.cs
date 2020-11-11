namespace Gruggbot.Data.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    internal class GruggbotContextFactory : IDesignTimeDbContextFactory<GruggbotContext>
    {
        public GruggbotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GruggbotContext>();
            optionsBuilder.UseSqlite("Data Source=..\\GruggbotDB.db");

            return new GruggbotContext(optionsBuilder.Options);
        }
    }
}
