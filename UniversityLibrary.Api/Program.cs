using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using UniversityLibrary.Api;
using UniversityLibrary.Api.Auth;
using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Model;
using UniversityLibrary.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UniversityLibrary.Api.Auth.Interfaces;
using UniversityLibrary.Api.Auth.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/students",
        async (IStudentService studentService) => await studentService.GetStudents())
    .WithName("Get Students");

app.MapGet("/students/{id}",
        async (CancellationToken token,
            IStudentService studentService,
            int id) => await studentService.GetStudent(id,
            token))
    .WithName("Get Student");

app.MapPost("/students/{id}",
        async (CancellationToken token,
            IStudentService studentService,
            CreateStudentDto student) => await studentService.CreateStudent(student,
            token))
    .WithName("Create Student");

app.MapPut("/students/{id}",
        async (CancellationToken token,
            IStudentService studentService,
            UpdateStudentDto student) => await studentService.UpdateStudent(student,
            token))
    .WithName("Update Student");

app.MapDelete("/students/{id}",
        async (CancellationToken token,
            IStudentService studentService,
            int id) => await studentService.DeleteStudent(id,
            token))
    .WithName("Delete Student");



app.MapGet("/user",
        [Authorize(Roles = "USER")] async (HttpContext context, IUserService userService) =>
        {
            // Retrieve userId from the claims
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
            Console.WriteLine("Claims received:");
            foreach (var claim in context.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            if (userIdClaim is null)
            {
                Console.WriteLine("No user ID claim present in token");
                return Results.Unauthorized();
            }
        
            try
            {
                var user = await userService.GetUser(userIdClaim);
                return Results.Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
    .WithName("Get User");

app.MapPost("/register",
        async (IUserService userService, ITokenService tokenService, RegistrationRequest request) =>
        {
            var salt = PasswordHasher.GenerateSalt();
            var user = new User
            {
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                Password = PasswordHasher.HashPassword(request.Password, salt), // Null is because the user is not created yet, normally this is where the user object is.
                Salt = salt,
                Role = Role.USER
            };

            await userService.CreateUser(user);
            var token = tokenService.CreateToken(user);

            return new AuthResponse { Token = token };
        })
    .WithName("Register user");

app.MapPost("/login",
        async (IUserService userService, ITokenService tokenService, LoginRequest request) =>
        {
            var user = await userService.FindByEmailAsync(request.Email); 

            if (user is null)
            {
                return Results.Unauthorized();
            }

            if (!PasswordHasher.VerifyPassword(user.Password,request.Password, user.Salt))
            {
                return Results.Unauthorized();
            }

            // Generate token
            var token = tokenService.CreateToken(user);

            // Return the token
            return Results.Ok(new AuthResponse { Token = token });
        })
    .WithName("Login");

app.Run();