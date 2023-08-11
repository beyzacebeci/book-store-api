using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public String BookName { get; set; }
        public decimal Price { get; set; }

        public string FileName { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        //navigation proporties
        public Author Author { get; set; }
        public Category Category { get; set; }
    }
}
