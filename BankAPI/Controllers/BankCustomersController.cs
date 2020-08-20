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
using SQLitePCL;

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

        // Get all user BankAccounts
        // GET: api/BankCustomers/5/passwordHash
        [HttpGet("{id:int}/{passwordHash}")]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankCustomer(uint id, string passwordHash)
        {
            var bankCustomer = await _context.BankCustomer.FindAsync(id);

            if (bankCustomer == null)
            {
                return NotFound();
            }

            if (bankCustomer.passwordHash != passwordHash)
            {
                // TODO: Raise the alarms
                return NotFound();
            }

            var bankAccounts = from bankAccount in _context.BankAccounts
                               where bankAccount.OwnerID == id
                               select bankAccount;

            return await bankAccounts.ToListAsync();
        }

        // Get valid BankCustomer from login
        // GET: api/BankCustomers/username/passwordHash
        [HttpGet("{username}/{passwordHash}")]
        public async Task<ActionResult<BankCustomer>> GetBankCustomer(string username, string passwordHash)
        {
            // fix for the base64 encoding
            passwordHash = Uri.UnescapeDataString(passwordHash);

            var bankAccount = from bankCustomer in _context.BankCustomer
                              where bankCustomer.Usernname == username && bankCustomer.passwordHash == passwordHash
                              select bankCustomer;

            var matchedAccounts = await bankAccount.ToArrayAsync();

            if (matchedAccounts == null || matchedAccounts.Length == 0)
            {
                return NotFound();
            }

            // do invalid passwords later
            //if (bankCustomer.passwordHash != passwordHash)
            //{
            //    // TODO: Raise the alarms
            //    return NotFound();
            //}

            return matchedAccounts[0];
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
