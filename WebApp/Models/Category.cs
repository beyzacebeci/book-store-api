using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }



        [JsonIgnore]
        public ICollection<Book> Books { get; set; }

        [JsonIgnore]
        public ICollection<Author> Authors { get; set; }


    }
}
