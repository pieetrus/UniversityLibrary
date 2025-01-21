using Microsoft.EntityFrameworkCore;
using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Services;

public class StudentService(DataContext dataContext) : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetStudents(CancellationToken token = default) =>
        await dataContext.Students
            .Include(s => s.LibraryCard)
            .Include(s => s.User)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                FirstName = s.User.Firstname,
                LastName = s.User.Lastname,
                StudentNumber = s.StudentNumber,
                LibraryCardId = s.LibraryCard == null ? null : s.LibraryCard.Id.ToString()
            })
            .ToListAsync(token);

    public async Task<StudentDto> GetStudent(int id, CancellationToken token = default)
    {
        var student = await dataContext.Students
            .Include(s => s.LibraryCard)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id, token);

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {id} not found");

        return new StudentDto
        {
            Id = student.Id,
            StudentNumber = student.StudentNumber,
            LibraryCardId = student.LibraryCard?.Id.ToString(),
            FirstName = student.User.Firstname,
            LastName = student.User.Lastname,
            UserId = student.UserId
        };

    }

    public async Task<StudentDto> CreateStudent(CreateStudentDto newStudent, CancellationToken token = default)
    {
        // Sprawdź czy user istnieje
        var user = await dataContext.Users
            .FirstOrDefaultAsync(u => u.Id == newStudent.UserId, token);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {newStudent.UserId} not found");

        var student = new Student
        {
            StudentNumber = newStudent.StudentNumber,
            UserId = newStudent.UserId
        };

        dataContext.Students.Add(student);
        await dataContext.SaveChangesAsync(token);

        return new StudentDto
        {
            Id = student.Id,
            StudentNumber = student.StudentNumber,
            LibraryCardId = null, // Nowy student nie ma jeszcze karty bibliotecznej
            UserId = student.UserId
        };
    }

    public async Task<StudentDto> UpdateStudent(UpdateStudentDto updatedStudent, CancellationToken token = default)
    {
        var student = await dataContext.Students
            .Include(s => s.LibraryCard)
            .FirstOrDefaultAsync(s => s.Id == updatedStudent.Id, token);

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {updatedStudent.Id} not found");

        student.StudentNumber = updatedStudent.StudentNumber;

        await dataContext.SaveChangesAsync(token);

        return new StudentDto
        {
            Id = student.Id,
            StudentNumber = student.StudentNumber,
            LibraryCardId = student.LibraryCard?.Id.ToString(),
            UserId = student.UserId
        };
    }

    public async Task<int> DeleteStudent(int id, CancellationToken token = default)
    {
        var student = await dataContext.Students
            .FirstOrDefaultAsync(s => s.Id == id, token);

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {id} not found");

        dataContext.Students.Remove(student);
        await dataContext.SaveChangesAsync(token);

        return id;
    }
}