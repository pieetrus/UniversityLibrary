namespace UniversityLibrary.Api.Model;

public class BookCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<Book> Books { get; set; }
}