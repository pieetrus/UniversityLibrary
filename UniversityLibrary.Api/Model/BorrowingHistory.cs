namespace UniversityLibrary.Api.Model;

public class BorrowingHistory
{
    public int Id { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    
    public int BookId { get; set; }
    public Book Book { get; set; }
    
    public int StudentId { get; set; }
    public Student Student { get; set; }
}