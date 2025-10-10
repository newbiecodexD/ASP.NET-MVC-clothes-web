using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using web_quanao.Core.Entities;

namespace web_quanao.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationDbContext()
        {
            // Apply pending migrations automatically (development scenario)
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, web_quanao.Migrations.Configuration>());
        }

        public ApplicationDbContext() : base("DefaultConnection") { }

        public static ApplicationDbContext Create() => new ApplicationDbContext();

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        // Removed Cart / CartItem DbSets (now in-memory only)

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ignore in-memory cart entities so EF does not try to migrate them
            modelBuilder.Ignore<Cart>();
            modelBuilder.Ignore<CartItem>();
        }
    }
}
