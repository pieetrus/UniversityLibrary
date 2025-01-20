namespace UniversityLibrary.Api.Model;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentNumber { get; set; }
    
    // Relacja 1-1
    public LibraryCard? LibraryCard { get; set; }
}