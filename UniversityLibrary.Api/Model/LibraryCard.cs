namespace UniversityLibrary.Api.Model;

public class LibraryCard
{
    public int Id { get; set; }
    public DateTime ValidUntil { get; set; }
    public bool IsActive { get; set; }
    
    // Klucz obcy do Student
    public int StudentId { get; set; }
    public Student Student { get; set; }
}