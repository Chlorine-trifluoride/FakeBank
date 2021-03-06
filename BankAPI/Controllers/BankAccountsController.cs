﻿using System;
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
    public class BankAccountsController : ControllerBase
    {
        private readonly BankContext _context;

        public BankAccountsController(BankContext context)
        {
            _context = context;
        }

        // GET: api/BankAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            return await _context.BankAccounts.ToListAsync();
        }

        // GET: api/BankAccounts/5/
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(uint id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);

            if (bankAccount == null)
            {
                return NotFound();
            }

            return bankAccount;
        }

        // Find if account exists based on IBAN
        // GET: api/BankAccounts/FA5354431166871100/
        [HttpGet("{rIBAN}")]
        public async Task<ActionResult<bool>> GetBankAccount(string rIBAN)
        {
            // TODO: this should be stored in DB instead
            (string BIC, string accountNumber) splitIBAN = Utils.GetBicAndAccFromIBAN(rIBAN);

            var result = from account in _context.BankAccounts
                         where account.BIC == splitIBAN.BIC && account.AccountNumber == splitIBAN.accountNumber
                         select account;

            if (result is null || result.Count() < 1)
            {
                return NotFound();
            }

            return true;
        }

        // Get bank accounts for specific user with password
        // GET: api/BankAccounts/5/passwordhash
        [HttpGet("{ownerID:int}/{passwordHash}")]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccount(uint ownerID, string passwordHash)
        {
            // fix for the base64 encoding
            passwordHash = Uri.UnescapeDataString(passwordHash);

            var user = from customer in _context.BankCustomer
                       where customer.ID == ownerID && customer.passwordHash == passwordHash
                       select customer;

            if (user.Count() < 1) // not found / wrong password
            {
                return Unauthorized();
            }

            var bankAccount = from account in _context.BankAccounts
                              where account.OwnerID == ownerID
                              select account;

            if (bankAccount == null)
            {
                return NotFound();
            }

            return await bankAccount.ToListAsync();
        }

        // PUT: api/BankAccounts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(uint id, BankAccount bankAccount)
        {
            if (id != bankAccount.ID)
            {
                return BadRequest();
            }

            _context.Entry(bankAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
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

        // POST: api/BankAccounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetBankAccount", new { id = bankAccount.ID }, bankAccount);
            return CreatedAtAction(nameof(GetBankAccount), new { id = bankAccount.ID }, bankAccount);
        }

        // DELETE: api/BankAccounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankAccount>> DeleteBankAccount(uint id)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(id);
            if (bankAccount == null)
            {
                return NotFound();
            }

            _context.BankAccounts.Remove(bankAccount);
            await _context.SaveChangesAsync();

            return bankAccount;
        }

        private bool BankAccountExists(uint id)
        {
            return _context.BankAccounts.Any(e => e.ID == id);
        }
    }
}
