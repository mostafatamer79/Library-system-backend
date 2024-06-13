using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowBookController : ControllerBase
    {
        private readonly ApplicationDbContext _content;


        public BorrowBookController(ApplicationDbContext context)
        {
            _content = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Borrow_Dto dto)
        {
            var isValidBookId= await _content.Books.AnyAsync(k=>k.id==dto.bookid);
            if (!isValidBookId)
            {
                return NotFound("the Book Not Found");
            }
            var isValidUserId = await _content.Users.AnyAsync(g => g.Id == dto.userid);
            if (!isValidUserId)
            {
                return NotFound("the User Not Found");
            }
          
           
                var book = new BorrowBook
                { 
                    userid = dto.userid,
                    bookid = dto.bookid,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(14),
                    acceptable = false

                };
                var bookcount = await _content.Books.SingleOrDefaultAsync(g => g.id == dto.bookid);

                await _content.AddAsync(book);
                _content.SaveChanges();

                return Ok(book);
            
            
                
        }
        [HttpGet]
        public async Task<IActionResult> ViewAsync()
        {
            var book = await _content.BorrowBooks.Include(m=>m.user).Include(m=>m.book).ToListAsync();
            return Ok(book);
        }
        [HttpGet("gellallthebookwiththeactive")]
        public async Task<IActionResult> GetAllBookBorrow()
        {
            var isvalid = await _content.BorrowBooks.Include(g => g.book).Include(g => g.user).Where(g=>g.acceptable==false).ToListAsync();


            return Ok(isvalid);
        }
        [HttpGet("getcountofborrow")]
        public async Task<IActionResult> getcountofborrow()
        {
            
            var isvalid = await _content.BorrowBooks.Include(g => g.book).Include(g => g.user).CountAsync();


            return Ok(isvalid);
        }
        [HttpGet("getcountofborrowaccepet")]
        public async Task<IActionResult> getcountofborrowaccpet()
        {

            var isvalid = await _content.BorrowBooks.Include(g => g.book).Include(g => g.user).Where(g => g.acceptable == false).CountAsync();


            return Ok(isvalid);
        }
        [HttpGet("getcountofborrowforiid/{id}")]
        public async Task<IActionResult> getcountofborrowforid(int id)
        {

            var isvalid = await _content.BorrowBooks.Include(g => g.book).Include(g => g.user).Where(g=>g.user.Id==id).CountAsync();


            return Ok(isvalid);
        }
        [HttpGet("getborrowbookwithid/{userid}")]
        public async Task<IActionResult> borrowwithid(int userid)
        {
            var isvalid = await _content.BorrowBooks.Include(g => g.book).Include(g => g.user).Where(g => g.userid == userid).ToListAsync();


            return Ok(isvalid);
        }
        [HttpPut("changetofalse/{id}")]
        public async Task<IActionResult> UpdatefalseAsync(int id)
        {
            var userid = await _content.BorrowBooks.SingleOrDefaultAsync(g => g.id == id);
            if (userid == null)
            {
                return NotFound($"no id was found with ID: {userid}");
            }
            userid.acceptable = false;
            _content.SaveChanges();
            return Ok(id);

        }
        [HttpPut("changetotrue/{id}")]
        public async Task<IActionResult> UpdatetrueAsync(int id)
        {
            var borrowBook = await _content.BorrowBooks
                                            .Include(b => b.book) 
                                            .SingleOrDefaultAsync(b => b.id == id);

            if (borrowBook == null)
            {
                return NotFound($"No BorrowBook was found with ID: {id}");
            }

            borrowBook.acceptable = true;

            if (borrowBook.book != null)
            {
                borrowBook.book.count -= 1;
            }

            try
            {
                await _content.SaveChangesAsync();
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating BorrowBook: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletAsync(int id)
        {
            var book = await _content.BorrowBooks.FindAsync(id);
            if (book == null)
            {
                return NotFound($"no movies was found with ID: {id}");
            }
            _content.Remove(book);
            _content.SaveChanges();
            return Ok(book);
        }
    }
}