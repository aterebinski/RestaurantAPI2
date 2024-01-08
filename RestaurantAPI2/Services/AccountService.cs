using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI2.Entities;
using RestaurantAPI2.Models;

namespace RestaurantAPI2.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO dto);
    }

    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(RestaurantDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public void RegisterUser(RegisterUserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            var passwordHash = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = passwordHash;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
