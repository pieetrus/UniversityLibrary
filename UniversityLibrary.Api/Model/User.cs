namespace UniversityLibrary.Api.Model;

public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public Role Role { get; set; }
    
    public Student? Student { get; set; }
    public int? StudentId { get; set; }
}