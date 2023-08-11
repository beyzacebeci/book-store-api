namespace WebApp.Data
{
    public class BookDto
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }
 
        public String CategoryName { get; set; }
        public String AuthorName { get; set; }

    }
}
