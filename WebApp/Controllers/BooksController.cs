using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {

            try
            {
                var booksDomain = _context.Books.Include(b => b.Author).Include(b => b.Category).ToList();

                var booksDto = new List<BookDto>();

                foreach (var bookDomain in booksDomain)
                {
                    booksDto.Add(new BookDto()
                    {
                        Id = bookDomain.BookId,
                        Title = bookDomain.BookName,
                        Price = bookDomain.Price,                                    
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
        public IActionResult CreateOneBook([FromBody] CreateBookDto book)
        {
            try
            {
                if (book is null)
                    return BadRequest();

                _context.Books.Add(new Book()
                {
                    AuthorId = book.AuthorId,
                    BookName = book.Name,
                    CategoryId = book.CategoryId,
                    Price = book.Price,
                });
                _context.SaveChanges();
                return StatusCode(200, book);
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
