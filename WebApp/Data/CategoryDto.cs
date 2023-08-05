﻿using System.Text.Json.Serialization;
using WebApp.Models;

namespace WebApp.Data
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ICollection<Book> Books { get; set; }
     
    }
}
