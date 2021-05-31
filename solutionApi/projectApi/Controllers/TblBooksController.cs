using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TblBooksController : ControllerBase
    {
        private readonly dbBooksEssaContext _context;

        public TblBooksController(dbBooksEssaContext context)
        {
            _context = context;
        }

        // GET: api/TblBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblBook>>> GetTblBooks(bool? inStock, int? skip, int? take)
        {
            var books = _context.TblBooks.AsQueryable();
            if (inStock != null)
            {
                books = _context.TblBooks.Where(i => i.Quantity > 0);
            }
            if (skip != null)
            {
                books = books.Skip((int)skip);
            }
            if (take != null)
            {
                books = books.Take((int)take);
            }

            return await books.ToListAsync();
        }

        // GET: api/TblBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblBook>> GetTblBook(int id)
        {
            var tblBook = await _context.TblBooks.FindAsync(id);

            if (tblBook == null)
            {
                return NotFound();
            }

            return tblBook;
        }

        // PUT: api/TblBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblBook(int id, TblBook tblBook)
        {
            if (id != tblBook.BookId)
            {
                return BadRequest();
            }

            _context.Entry(tblBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TblBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TblBook>> PostTblBook(TblBook tblBook)
        {
            _context.TblBooks.Add(tblBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblBook", new { id = tblBook.BookId }, tblBook);
        }

        // DELETE: api/TblBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblBook(int id)
        {
            var tblBook = await _context.TblBooks.FindAsync(id);
            if (tblBook == null)
            {
                return NotFound();
            }

            _context.TblBooks.Remove(tblBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblBookExists(int id)
        {
            return _context.TblBooks.Any(e => e.BookId == id);
        }
    }
}
