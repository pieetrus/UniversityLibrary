namespace UniversityLibrary.Api.Dtos;

public class CreateStudentDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentNumber { get; set; }
    public int UserId { get; set; }  // Dodane pole
}