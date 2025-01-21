using Microsoft.EntityFrameworkCore;
using UniversityLibrary.Api.Auth.Interfaces;
using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Auth;

public class UserService(DataContext context) : IUserService
{
    public async Task CreateUser(User user)
    {
        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);
            
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {user.Email} already exists");

        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task<User?> FindByEmailAsync(string requestEmail)
    {
        return await context.Users
            .Include(u => u.Student)  // Dodajemy Include żeby załadować powiązanego studenta
            .FirstOrDefaultAsync(u => u.Email == requestEmail);
    }

    public async Task<UserDto?> GetUser(string? userIdClaim)
    {
        if (userIdClaim == null)
            return null;
        
        if (!int.TryParse(userIdClaim, out var userId))
            return null;

        var userDb = await context.Users
            .Include(u => u.Student)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        if (userDb == null)
            return null;

        return new UserDto
        {
            Id = userDb.Id,
            Email = userDb.Email,
            StudentId = userDb.StudentId,
            Firstname = userDb.Firstname,
            Lastname = userDb.Lastname,
            StudentNumber = userDb.Student?.StudentNumber,
            Role = userDb.Role,
        };
    }
}