using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public CategoriesController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categoriesDomain = _context.Categories
                     .Include(b => b.Books)
                     .ThenInclude(b => b.Author)
                     .ToList();
                var categoriesDto = new List<CategoryDto>();

                foreach (var categoryDomain in categoriesDomain)
                {
                    categoriesDto.Add(new CategoryDto()
                    {
                        Id=categoryDomain.CategoryId,
                        Name = categoryDomain.CategoryName,
                        Books = categoryDomain.Books.ToList(),

                    });
                }
                return Ok(categoriesDto);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        [HttpGet("{id:int}")]

        public IActionResult GetOneCategory([FromRoute(Name = "id")] int id)
        {
            try
            {
                var categoryDomain = _context
                    .Categories
                    .Include(b => b.Books)
                    .Where(b => b.CategoryId.Equals(id))
                    .SingleOrDefault();

                if (categoryDomain is null)
                    return NotFound();

                var categoryDto = new CategoryDto()
                {
                    Id = categoryDomain.CategoryId,
                    Name = categoryDomain.CategoryName,
                    Books = categoryDomain.Books.ToList()

                };
                return Ok(categoryDto);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        [HttpGet("{categoryName}")]
        public IActionResult GetBooksByCategory([FromRoute(Name = "categoryName")] string categoryName)
        {
            try
            {
                IQueryable<Book> booksQuery = _context.Books.Include(b => b.Author).Include(b => b.Category);

                if (!string.IsNullOrEmpty(categoryName))
                {
                    booksQuery = booksQuery.Where(b => b.Category.CategoryName == categoryName);
                }

                var bookDomains = booksQuery.ToList();

                if (bookDomains.Count == 0)
                    return NotFound();

                
     
              
                
                var bookDtos = bookDomains.Select(bookDomain => new BookDto()
                {
                    
                    Id = bookDomain.BookId,
                    Title = bookDomain.BookName,
                    Price = bookDomain.Price,
                    AuthorName = bookDomain.Author.AuthorName,
                    CategoryId = bookDomain.Category.CategoryId,
                    CategoryName = bookDomain.Category.CategoryName
                }).ToList();

                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        public IActionResult CreateOneCategory([FromBody] Category category)
        {
            try
            {
                if (category is null)
                    return BadRequest();

                _context.Categories.Add(category);
                _context.SaveChanges();
                return StatusCode(201,category);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneCategory([FromRoute(Name = "id")] int id, [FromBody] Category category)
        {

            try
            {
                //check category
                var entity = _context
                    .Categories
                    .Where(b => b.CategoryId.Equals(id))
                    .SingleOrDefault();

                if (entity is null) return NotFound();

                //check id

                if (id != category.CategoryId)
                    return BadRequest();


                entity.CategoryName = category.CategoryName;

                _context.SaveChanges();

                return Ok(category);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

 
        [HttpDelete("{id:int}")]

        public IActionResult DeleteOneCategory([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context
                            .Categories
                            .Where(b => b.CategoryId.Equals(id))
                            .SingleOrDefault();
                if (entity is null)
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = $"Category with id:{id} could not found"
                    });

                _context.Categories.Remove(entity);
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
