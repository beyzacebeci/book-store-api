using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }
        public String AuthorName { get; set; }
        public string FileName { get; set; }
        public ICollection<Category> Categories { get; set; }

        //navigation proporties
        [JsonIgnore]
        public ICollection<Book> Books { get; set; }
    }
}
