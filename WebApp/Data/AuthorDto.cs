using WebApp.Models;

namespace WebApp.Data
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public List<string> BookNames { get; set; }


        //public ICollection<Book> Books { get; set; }

        //public ICollection<Category> Categories { get; set; }

    }
}
