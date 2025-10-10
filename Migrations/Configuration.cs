using System.Data.Entity.Migrations;
using web_quanao.Infrastructure.Data;

namespace web_quanao.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            // Enable automatic migrations to sync model changes (development only)
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false; // keep false to avoid unintended drops
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // Seed initial data if needed.
        }
    }
}
