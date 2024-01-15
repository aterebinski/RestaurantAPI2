using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI2.Authorization;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Exceptions;
using RestaurantAPI2.Models;
using System.Security.Claims;

namespace RestaurantAPI2.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDTO dto);
        List<RestaurantDTO> GetAll(string searchPhrase);
        RestaurantDTO GetById(int Id);
        public void Delete(int id);
        public void Update(int id, EditRestaurantDTO dto);

    }

    public class RestaurantService : IRestaurantService
    {
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly RestaurantDbContext DbContext;
        private readonly IMapper _mapper;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, 
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            DbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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

        public List<RestaurantDTO> GetAll(string searchPhrase)
        {
            var restaurants = DbContext
                .Restaurants
                .Include(c => c.Address)
                .Include(c => c.Dishes)
                .Where(i => searchPhrase == null || (i.Name.ToLower().Contains(searchPhrase.ToLower())||i.Name.Contains(searchPhrase.ToLower())))
                .ToList();
            return _mapper.Map<List<RestaurantDTO>>(restaurants);
        }

        public int Create(CreateRestaurantDTO dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.getUserId;
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

            ClaimsPrincipal User = _userContextService.User;
            var authorizationResult = _authorizationService.AuthorizeAsync(User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            DbContext.Remove(restaurant);
            DbContext.SaveChanges();
        }

        public void Update(int id, EditRestaurantDTO dto)
        {
            var restaurant = DbContext.Restaurants.FirstOrDefault(i => i.Id == id);
            if (restaurant is null)
                throw new NotFoundException("REstaurant not found");

            ClaimsPrincipal User = _userContextService.User;

            var result = _authorizationService.AuthorizeAsync(User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!result.Succeeded)
            {
                throw new ForbiddenException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;
            DbContext.SaveChanges();
        }
    }
}
