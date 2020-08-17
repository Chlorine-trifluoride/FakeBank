using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAPI;
using BankAPI.Models;
using BankModel;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankCustomersController : ControllerBase
    {
        private readonly BankContext _context;

        public BankCustomersController(BankContext context)
        {
            _context = context;
        }

        // GET: api/BankCustomers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankCustomer>>> GetBankCustomer()
        {
            return await _context.BankCustomer.ToListAsync();
        }

        // GET: api/BankCustomers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankCustomer>> GetBankCustomer(uint id)
        {
            var bankCustomer = await _context.BankCustomer.FindAsync(id);

            if (bankCustomer == null)
            {
                return NotFound();
            }

            return bankCustomer;
        }

        // PUT: api/BankCustomers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankCustomer(uint id, BankCustomer bankCustomer)
        {
            if (id != bankCustomer.ID)
            {
                return BadRequest();
            }

            _context.Entry(bankCustomer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankCustomerExists(id))
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

        // POST: api/BankCustomers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankCustomer>> PostBankCustomer(BankCustomer bankCustomer)
        {
            _context.BankCustomer.Add(bankCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankCustomer", new { id = bankCustomer.ID }, bankCustomer);
        }

        // DELETE: api/BankCustomers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankCustomer>> DeleteBankCustomer(uint id)
        {
            var bankCustomer = await _context.BankCustomer.FindAsync(id);
            if (bankCustomer == null)
            {
                return NotFound();
            }

            _context.BankCustomer.Remove(bankCustomer);
            await _context.SaveChangesAsync();

            return bankCustomer;
        }

        private bool BankCustomerExists(uint id)
        {
            return _context.BankCustomer.Any(e => e.ID == id);
        }
    }
}
