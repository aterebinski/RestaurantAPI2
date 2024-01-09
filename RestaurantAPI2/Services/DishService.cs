using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Exceptions;
using RestaurantAPI2.Models;


namespace RestaurantAPI2.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDTO dto);
        IEnumerable<Dish> GetAll(int restaurantid);
        Dish getById(int restaurantId, int dishId);

    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<Dish> GetAll(int restaurantid)
        {
            var restaurant = getRestauantById(restaurantid);
            var dishes = restaurant.Dishes.ToList();
            return dishes;
        }

        public Dish getById(int restaurantId, int dishId)
        {
            var restaurant = getRestauantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (dish is null)
                throw new NotFoundException("Dish not found");
            return dish;
        }

        private Restaurant getRestauantById(int restaurantId)
        {
            Restaurant restaurant = _dbContext.Restaurants
                .Include(d=>d.Dishes)
                .FirstOrDefault(i => i.Id == restaurantId);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            return restaurant;
        }

        public int Create(int restaurantId, CreateDishDTO dto)
        {
            var restaurant = getRestauantById(restaurantId);
            Dish dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;
            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();
            return dish.Id;
        }

    }   
}
