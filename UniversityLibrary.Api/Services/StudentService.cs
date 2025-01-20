using Microsoft.EntityFrameworkCore;
using UniversityLibrary.Api.Dtos;
using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Services;

public class StudentService(DataContext dataContext) : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetStudents(CancellationToken token = default) =>
        await dataContext.Students
            .Include(s => s.LibraryCard)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                StudentNumber = s.StudentNumber,
                LibraryCardId = s.LibraryCard.Id.ToString()
            })
            .ToListAsync(token);

    public async Task<StudentDto> GetStudent(int id, CancellationToken token = default)
    {
        var student = await dataContext.Students
            .Include(s => s.LibraryCard)
            .FirstOrDefaultAsync(s => s.Id == id, token);

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {id} not found");

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            StudentNumber = student.StudentNumber,
            LibraryCardId = student.LibraryCard?.Id.ToString()
        };

    }

    public async Task<StudentDto> CreateStudent(CreateStudentDto newStudent, CancellationToken token = default)
    {
        var student = new Student
        {
            FirstName = newStudent.FirstName,
            LastName = newStudent.LastName,
            StudentNumber = newStudent.StudentNumber
        };

        dataContext.Students.Add(student);
        await dataContext.SaveChangesAsync(token);

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            StudentNumber = student.StudentNumber,
            LibraryCardId = null // Nowy student nie ma jeszcze karty bibliotecznej
        };
    }

    public async Task<StudentDto> UpdateStudent(UpdateStudentDto updatedStudent, CancellationToken token = default)
    {
        var student = await dataContext.Students
            .Include(s => s.LibraryCard)
            .FirstOrDefaultAsync(s => s.Id == updatedStudent.Id, token);

        if (student == null)
            throw new KeyNotFoundException($"Student with ID {updatedStudent.Id} not found");

        student.FirstName = updatedStudent.FirstName;
        student.LastName = updatedStudent.LastName;
        student.StudentNumber = updatedStudent.StudentNumber;

        await dataContext.SaveChangesAsync(token);

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            StudentNumber = student.StudentNumber,
            LibraryCardId = student.LibraryCard?.Id.ToString()
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