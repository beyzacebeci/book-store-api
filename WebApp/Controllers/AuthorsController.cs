using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public AuthorsController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
           try
            {
                var authorsDomain = _context.Authors
                    .Include(b => b.Books)
                    //.Include(b=>b.Categories)
                    .ToList();
               
                var authorsDto = new List<AuthorDto>();

                foreach (var authorDomain in authorsDomain)
                {
                    authorsDto.Add(new AuthorDto()
                    {
                        Id = authorDomain.AuthorId,
                        Name = authorDomain.AuthorName,
                        //Books = authorDomain.Books.ToList()
                        BookNames = authorDomain.Books.Select(book => book.BookName).ToList()
                    });
                }
                return Ok(authorsDto);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
      
        [HttpGet("{id:int}")]
        public IActionResult GetOneAuthor([FromRoute(Name = "id")] int id)
        {

            try
            {
                var authorDomain = _context
                    .Authors
                    .Include(b => b.Books)
                    .Where(b => b.AuthorId.Equals(id))
                    .SingleOrDefault();

                if (authorDomain is null)
                    return NotFound();


                var authorDto = new AuthorDto()
                {
                    Id = authorDomain.AuthorId,
                    Name = authorDomain.AuthorName,
                    BookNames = authorDomain.Books.Select(book => book.BookName).ToList()

                    //Books = authorDomain.Books.ToList(),
                    //Categories = authorDomain.Categories.ToList()

                };

                return Ok(authorDto);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult CreateOneAuthor([FromBody] CreateAuthorDto author)
        {
            try
            {
                if (author is null)
                    return BadRequest();

                _context.Authors.Add(new Author
                {
                    AuthorId = author.AuthorId,
                    AuthorName=author.Name

                }) ;

           
                _context.SaveChanges();
                return StatusCode(201, author);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneAuthor([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context
                            .Authors
                            .Where(b => b.AuthorId.Equals(id))
                            .SingleOrDefault();
                if (entity is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = $"Author with id:{id} could not found"
                    });

                _context.Authors.Remove(entity);
                _context.SaveChanges();
                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneAuthor([FromRoute(Name = "id")] int id, [FromBody] Author author)
        {
            try
            {
                var entity = _context
                    .Authors
                    .Where(b => b.AuthorId.Equals(id))
                    .SingleOrDefault();
                if (entity is null)
                    return NotFound();

                if (id != author.AuthorId)
                    return BadRequest();

                entity.AuthorName = author.AuthorName;
                _context.SaveChanges();

                return Ok(author);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


    }
}
