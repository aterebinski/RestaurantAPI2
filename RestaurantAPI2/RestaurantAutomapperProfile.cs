using AutoMapper;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Models;

namespace RestaurantAPI2
{
    public class RestaurantAutomapperProfile : Profile
    {
        public RestaurantAutomapperProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(n=>n.Street,z=>z.MapFrom(a=>a.Address.Street))
                .ForMember("PostalCode",z=>z.MapFrom(a=>a.Address.PostalCode))
                .ForMember(n=>n.City,z=>z.MapFrom(a=>a.Address.City));

            CreateMap<Dish, DishDTO>();

            CreateMap<CreateRestaurantDTO, Restaurant>()
                .ForMember(a => a.Address, b => b.MapFrom(c => new Address() { City = c.City, PostalCode = c.PostalCode, Street = c.Street }));

            CreateMap<EditRestaurantDTO, Restaurant>();

            CreateMap<CreateDishDTO, Dish>();

            CreateMap<RegisterUserDTO, User>();
        }
    }
}
