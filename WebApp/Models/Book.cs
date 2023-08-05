namespace WebApp.Models
{
    public class Book
    {

        public int BookId { get; set; }

        public String BookName { get; set; }

        public decimal Price { get; set; }


        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        //navigation proporties
        public Author Author { get; set; }
        public Category Category { get; set; }



    }
}
