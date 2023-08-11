using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthorsController(RepositoryContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                    string base64Image = null;
                    if (System.IO.File.Exists(authorDomain.FileName))
                    {
                        byte[] b = System.IO.File.ReadAllBytes(authorDomain.FileName);
                        base64Image= Convert.ToBase64String(b);                  
                    }
                    authorsDto.Add(new AuthorDto()
                    {
                        Id = authorDomain.AuthorId,
                        Name = authorDomain.AuthorName,
                        //Books = authorDomain.Books.ToList()
                        BookNames = authorDomain.Books.Select(book => book.BookName).ToList(),
                        Image = base64Image
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
        public IActionResult CreateOneAuthor([FromForm] CreateAuthorDto input)
        {
            try
            {
                if (input is null)
                    return BadRequest();

                string path = null;

                if (input.Files.Length > 0)
                {
                    path = _webHostEnvironment.WebRootPath + "\\uploads\\authors\\";

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


                _context.Authors.Add(new Author
                {
                    AuthorName = input.Name,
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

        //[HttpPut("{id:int}")]
        //public IActionResult UpdateOneAuthor([FromRoute(Name = "id")] string id, [FromForm] CreateAuthorDto input)
        //{
        //    try
        //    {
        //        var entity = _context
        //            .Authors
        //            .Where(b => b.AuthorId.Equals(id))
        //            .SingleOrDefault();
        //        if (entity is null)
        //            return NotFound();

        //        string path = null;

        //        if (input.Files.Length > 0)
        //        {
        //            path = _webHostEnvironment.WebRootPath + "\\uploads\\authors\\";

        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }

        //            using (FileStream fileStream = System.IO.File.Create(path + input.Files.FileName))
        //            {

        //                input.Files.CopyTo(fileStream);
        //                fileStream.Flush();

        //            }
        //        }


        //        //if (id != input.Name)
        //        //    return BadRequest();

        //        entity.AuthorName= input.Name;
        //        entity.FileName = path + "\\" + input.Files.FileName;
        //        _context.SaveChanges();

        //        return Ok(input);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }


        //}




    }
}
