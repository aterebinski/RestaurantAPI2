using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Exceptions;
using RestaurantAPI2.Models;

namespace RestaurantAPI2.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDTO dto);
        List<RestaurantDTO> GetAll();
        RestaurantDTO GetById(int Id);
        public void Delete(int id);
        public void Update(int id, EditRestaurantDTO dto);

    }

    public class RestaurantService : IRestaurantService
    {
        private readonly ILogger<RestaurantService> _logger;

        private readonly RestaurantDbContext DbContext;
        private readonly IMapper _mapper;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            DbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }



        public RestaurantDTO? GetById(int Id)
        {
            var restaurant = DbContext.Restaurants
                .Include(c => c.Address)
                .Include(c => c.Dishes)
                .FirstOrDefault(x => x.Id == Id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found!");

            return _mapper.Map<RestaurantDTO>(restaurant);

        }

        public List<RestaurantDTO> GetAll()
        {
            var restaurants = DbContext
                .Restaurants
                .Include(c => c.Address)
                .Include(c => c.Dishes)
                .ToList();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public int Create(CreateRestaurantDTO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            DbContext.Add(restaurant);
            DbContext.SaveChanges();
            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE acrion involved");

            var restaurant = DbContext.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            DbContext.Remove(restaurant);
            DbContext.SaveChanges();
        }

        public void Update(int id, EditRestaurantDTO dto)
        {
            var restaurant = DbContext.Restaurants.FirstOrDefault(i => i.Id == id);
            if (restaurant is null)
                throw new NotFoundException("REstaurant not found");
            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;
            DbContext.SaveChanges();
        }
    }
}
