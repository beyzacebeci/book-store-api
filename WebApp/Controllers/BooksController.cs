using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BooksController(RepositoryContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {

            try
            {
                var booksDomain = _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .ToList();

                var booksDto = new List<BookDto>();

                foreach (var bookDomain in booksDomain)
                {

                    string base64Image = null;
                    if (System.IO.File.Exists(bookDomain.FileName))
                    {
                        byte[] b = System.IO.File.ReadAllBytes(bookDomain.FileName);
                        base64Image = Convert.ToBase64String(b);
                    }

                    booksDto.Add(new BookDto()
                    {
                        Id = bookDomain.BookId,
                        Title = bookDomain.BookName,
                        Price = bookDomain.Price,     
                        Image=base64Image,
                        AuthorName = bookDomain.Author.AuthorName,
                        CategoryName = bookDomain.Category.CategoryName,
                        CategoryId = bookDomain.Category.CategoryId,

                    });
                }


                return Ok(booksDto);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {

            try
            {
                var bookDomain = _context
                    .Books
                    .Include(b => b.Author).Include(b => b.Category)
                    .Where(b => b.BookId.Equals(id))
                    .SingleOrDefault();

                if (bookDomain is null)
                    return NotFound();

                var bookDto = new BookDto()
                {
                    Id = bookDomain.BookId,
                    Title = bookDomain.BookName,
                    Price = bookDomain.Price,
                    AuthorName = bookDomain.Author.AuthorName,
                    CategoryId = bookDomain.Category.CategoryId,
                    CategoryName = bookDomain.Category.CategoryName


                };
                
                return Ok(bookDto);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }




        }



        [HttpPost]
        public IActionResult CreateOneBook([FromForm] CreateBookDto input)
        {
            try
            {
                if (input is null)
                    return BadRequest();

                string path = null;


                if (input.Files.Length > 0)
                {
                    path = _webHostEnvironment.WebRootPath + "\\uploads\\books\\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + input.Files.FileName))
                    {

                        input.Files.CopyTo(fileStream);
                        fileStream.Flush();

                    }
                }
                _context.Books.Add(new Book()
                {
                    AuthorId = input.AuthorId,            
                    BookName = input.Name,
                    CategoryId = input.CategoryId,
                    Price = input.Price,
                    FileName = path + "\\" + input.Files.FileName

                });
                _context.SaveChanges();
                return Ok(input);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {

            try
            {
                //check book
                var entity = _context
                    .Books
                    .Where(b => b.BookId.Equals(id))
                    .SingleOrDefault();

                if (entity is null) return NotFound();

                //check id

                if (id != book.BookId)
                    return BadRequest();


                entity.BookName = book.BookName;
                entity.Price = book.Price;


                _context.SaveChanges();

                return Ok(book);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context
                            .Books
                            .Where(b => b.BookId.Equals(id))
                            .SingleOrDefault();
                if (entity is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = $"Book with id:{id} could not found"
                    });

                _context.Books.Remove(entity);
                _context.SaveChanges();
                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



    }
}
