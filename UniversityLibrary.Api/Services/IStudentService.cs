using UniversityLibrary.Api.Dtos;

namespace UniversityLibrary.Api.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetStudents(CancellationToken token = default);
    Task<StudentDto> GetStudent(int id, CancellationToken token = default);
    Task<StudentDto> CreateStudent(CreateStudentDto newStudent, CancellationToken token = default);
    Task<StudentDto> UpdateStudent(UpdateStudentDto updatedStudent, CancellationToken token = default);
    Task<int> DeleteStudent(int id, CancellationToken token = default);
}