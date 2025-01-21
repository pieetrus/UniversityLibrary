using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Auth.Interfaces;

public interface IUserService
{
    Task CreateUser(User user);
    Task<User?> FindByEmailAsync(string requestEmail);
    Task<UserDto?> GetUser(string? userIdClaim);
}