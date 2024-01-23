using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI2.Entities
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
                
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property("Name")
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property("Name")
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property("City")
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property("Street")
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property("Email")
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property("Name")
                .IsRequired();

        }

    }
}
