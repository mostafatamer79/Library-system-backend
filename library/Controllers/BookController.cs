using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _content;

        private new List<string> _allowedExtensions = new List<string> { ".jpg", "png", ".jpeg" };
        private long maxAllowedPosterSize = 1048576;
        public BookController(ApplicationDbContext context)
        {
            _content = context;
        }

        [HttpGet]
        public async Task<IActionResult> ViewAsync()
        {
            
            var book = await _content.Books.ToListAsync();
            return Ok(book);
        }
        [HttpGet("validbook")]
        public async Task<IActionResult> ViewtheAvilableBook()
        {

            var book = await _content.Books.Where(g=>g.count>0).ToListAsync();
            return Ok(book);
        }
        [HttpGet("available-count")]
        public async Task<IActionResult> GetAvailableBookCount()
        {
            try
            {
                
                var availableBookCount = await _content.Books.CountAsync(b => b.count != 0);
                return Ok(availableBookCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("gellallthebookwiththid/{id}")]
        public async Task<IActionResult> GetAllBookBorrow(int id)
        {
            var isvalid = await _content.Books.Where(g => g.id == id).ToListAsync();


            return Ok(isvalid);
        }
        [HttpPost("addbook")]
        public async Task<IActionResult> CreateAsync([FromForm] Book_Dto dto)
        {
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg");
            if (dto.poster.Length > maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster 1MB");

            // Save the file to a specific location
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.poster.FileName);
            string path = Path.Combine(@"D:\ia\library\library (12)\src\images", fileName);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await dto.poster.CopyToAsync(stream);
            }

            // Create a new Book entity with the file path
            var book = new Book
            {
                
                Author = dto.Author,
                Title = dto.Title,
                Genre = dto.Genre,
                rate = dto.rate,
                poster = path, // Save the file path to the database
                status = true,
                count = dto.count
            };

            // Add the book to the database
            await _content.AddAsync(book);
            await _content.SaveChangesAsync();

            return Ok(book);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] Book_Dto dto)
        {
            var book = await _content.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"No book was found with ID: {id}");
            }

            // Handle new poster file if provided
            if (dto.poster != null)
            {
                // Validate file extension and size
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.poster.FileName).ToLower()))
                {
                    return BadRequest("Only .png and .jpg files are allowed.");
                }

                if (dto.poster.Length > maxAllowedPosterSize)
                {
                    return BadRequest("Max allowed size for poster is 1MB.");
                }

                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.poster.FileName);
                string filePath = Path.Combine(@"D:\ia\library\library (12)\src\images", fileName);

                // Save the new poster file to the specified path
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.poster.CopyToAsync(stream);
                }

                // Remove the old poster file if it exists
                if (!string.IsNullOrEmpty(book.poster) && System.IO.File.Exists(book.poster))
                {
                    System.IO.File.Delete(book.poster);
                }

                // Update the book with the new poster path
                book.poster = filePath;
            }

            // Update other book properties
            book.Author = dto.Author;
            book.Title = dto.Title;
            book.Genre = dto.Genre;
            book.rate = dto.rate;
            book.Genre = dto.Genre;
            book.count = dto.count;

            // Save changes to the database
            await _content.SaveChangesAsync();

            return Ok(book);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var book = await _content.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound($"No book was found with ID: {id}");
            }

            // Remove the poster file associated with the book
            if (!string.IsNullOrEmpty(book.poster) && System.IO.File.Exists(book.poster))
            {
                System.IO.File.Delete(book.poster);
            }

            // Remove the book entity from the context
            _content.Books.Remove(book);
            await _content.SaveChangesAsync();

            return Ok(book); // Returning the deleted book as a response (optional)
        }


    }
}
