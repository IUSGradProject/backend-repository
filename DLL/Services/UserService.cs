﻿using AutoMapper;
using BLL.Services.Interface;
using APIs.Contracts;
using APIs.Repository.Interface;
using DTOs.Contracts;
using APIs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<string> CreateUser(CreateUserRequest user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var model = _mapper.Map<User>(user);
            model.RoleId = 1;
            model.IsDeleted = false;
            if (string.IsNullOrWhiteSpace(model.Username))
            {
                model.Username = null; // Or derive a username from email if needed
            }
            await _userRepository.CreateUser(model);

            var createdUser = await _userRepository.GetUserByEmailAsync(user.Email);

            var token = _tokenService.GenerateToken(createdUser);
            return token;
        }

        public Task<IEnumerable<User>> GetInactiveUsers(DateTime dateTime)
        {
            return _userRepository.GetInactiveUsersAsync(dateTime);
        }

        public async Task<Role> GetRole(string email)
        {
            var role = await _userRepository.GetRole(email);
           // var response = _mapper.Map<RoleResponse>(role);
            return role;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = _userRepository.GetUserByEmailAsync(loginRequest.Email).Result;
            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            if (user.IsDeleted)
            {
                throw new Exception("This account has been deactivated. Please contact support.");
            }

            if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                var token = _tokenService.GenerateToken(user);

                var role = user.Role?.Role1;

                return new LoginResponse(token, user.Email, user.FirstName, user.LastName, user.Username, role);

            }

            throw new Exception("Invalid login");
        }

        public async Task LogUserActivity(string email)
        {
            await _userRepository.LogUserActivity(email);
        }

        public async Task DeactivateUserAsync(string email)
        {
            await _userRepository.DeactivateUserAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
    }
}
