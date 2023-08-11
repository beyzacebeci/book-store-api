namespace WebApp.Data
{
    public class CreateBookDto
    {
        public String Name { get; set; }
        public decimal Price { get; set; }

        public IFormFile Files { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
