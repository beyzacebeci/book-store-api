using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public String AuthorName { get; set; }


        public ICollection<Category> Categories { get; set; }

        //navigation proporties

        [JsonIgnore]
        public ICollection<Book> Books { get; set; }

    }
}
