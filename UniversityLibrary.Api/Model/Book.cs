namespace UniversityLibrary.Api.Model;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    
    // Klucz obcy do kategorii
    public int BookCategoryId { get; set; }
    public BookCategory Category { get; set; }
    
    // Relacja 1-*
    public ICollection<BorrowingHistory> BorrowingHistory { get; set; }
}