using UniversityLibrary.Api;
using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IStudentService, StudentService>();

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

app.Run();

