namespace UniversityLibrary.Api.Model;

public class Student
{
    public int Id { get; set; }
    public string StudentNumber { get; set; }
    public LibraryCard? LibraryCard { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
}