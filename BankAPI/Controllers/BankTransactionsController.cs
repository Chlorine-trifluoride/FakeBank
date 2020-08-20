using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAPI.Models;
using BankModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankTransactionsController : ControllerBase
    {
        private readonly BankContext _context;

        public BankTransactionsController(BankContext context)
        {
            _context = context;
        }

        // GET: api/BankTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankTransaction>>> GetBankTransaction()
        {
            return await _context.BankTransaction.ToListAsync();
        }

        // GET: api/BankTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankTransaction>> GetBankTransaction(uint id)
        {
            var bankTransaction = await _context.BankTransaction.FindAsync(id);

            if (bankTransaction == null)
            {
                return NotFound();
            }

            return bankTransaction;
        }

        // PUT: api/BankTransactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankTransaction(uint id, BankTransaction bankTransaction)
        {
            if (id != bankTransaction.ID)
            {
                return BadRequest();
            }

            _context.Entry(bankTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankTransactionExists(id))
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

        // POST: api/BankTransactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankTransaction>> PostBankTransaction(BankTransaction bankTransaction)
        {
            BankAccount sender = (from acc in _context.BankAccounts
                                 where acc.IBAN == bankTransaction.SenderIBAN
                                 select acc).First();

            BankAccount reciever = (from acc in _context.BankAccounts
                                  where acc.IBAN == bankTransaction.RecieverIBAN
                                  select acc).First();

            // Validate the transaction request
            if (sender.IsFrozen || sender.Balance < bankTransaction.Amount)
            {
                return Conflict();
            }

            // Move the currency
            sender.Balance -= (int)bankTransaction.Amount;
            // This is where we would deduct all the fees to fund our space program
            reciever.Balance += (int)bankTransaction.Amount;

            // Log the transaction
            _context.BankTransaction.Add(bankTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBankTransaction", new { id = bankTransaction.ID }, bankTransaction);
        }

        // DELETE: api/BankTransactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankTransaction>> DeleteBankTransaction(uint id)
        {
            var bankTransaction = await _context.BankTransaction.FindAsync(id);
            if (bankTransaction == null)
            {
                return NotFound();
            }

            _context.BankTransaction.Remove(bankTransaction);
            await _context.SaveChangesAsync();

            return bankTransaction;
        }

        private bool BankTransactionExists(uint id)
        {
            return _context.BankTransaction.Any(e => e.ID == id);
        }
    }
}
