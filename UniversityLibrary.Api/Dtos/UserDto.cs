using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    
    public string? StudentNumber { get; set; }

    public int? StudentId { get; set; }
}