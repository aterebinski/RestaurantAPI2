using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Entities;

namespace RestaurantAPI2
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void Seed()
        {
            if (!_dbContext.Restaurants.Any())
            {
                //pobierz listę migracji które nie zostały jeszce wykonane
                var pendingMigrations  = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any()) 
                {
                    _dbContext.Database.Migrate();
                }


                IEnumerable<Restaurant> restaurants = GetRestaurants();
                _dbContext.Restaurants.AddRange(restaurants);
                _dbContext.SaveChanges();
            }


            if (!_dbContext.Roles.Any())
            {
                IEnumerable<Role> roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User",
                },
                new Role()
                {
                    Name = "Manager",
                },
                new Role()
                {
                    Name = "Admin",
                }
            };
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>() 
            {
                new Restaurant()
                {
                    Name = "McDonalds",
                    Description = "Restauracja McDonalds",
                    Category = "FastFood",
                    HasDelivery = true,
                    ContactEmail = "contact@mcdonalds.com",
                    ContactNumber = "+48897541222",
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Frytki",
                            Description = "Frytki na ciepło",
                            Price = 6,
                        },
                        new Dish()
                        {
                            Name = "Cheesburger",
                            Description = "Burger z serem",
                            Price= 4,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Nashville",
                        PostalCode = "62510",
                        Street = "Elvis Presley Street 1A"
                    }
                },
                new Restaurant()
                {
                    Name = "KFC",
                    Description = "Kentucky Fried Chicken",
                    Category = "FastFood",
                    HasDelivery = true,
                    ContactEmail = "contact@kfc.com",
                    ContactNumber = "+48897891222",
                    Address = new Address()
                    {
                        City = "Dallas",
                        PostalCode = "62500",
                        Street = "John Carpenter Street 97"
                    }
                }
            };

            return restaurants;
        }
    }
}
