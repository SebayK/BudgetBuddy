﻿using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class TransactionService {
  private readonly BudgetContext _context;
  public readonly CurrencyConverterService _converter;

  public TransactionService(BudgetContext context, CurrencyConverterService converter) {
    _context = context;
    _converter = converter;
  }

  public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync() {
    return await _context.Transaction.AsNoTracking().ToListAsync();
  }

  public async Task<Transaction?> GetTransactionByIdAsync(int id) {
    return await _context.Transaction.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
  }

  public async Task<bool> UpdateTransactionAsync(int id, Transaction transaction) {
    if (id != transaction.Id) return false;

    _context.Entry(transaction).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await TransactionExistsAsync(id)) return false;
      throw;
    }
  }

  public async Task<Transaction> CreateTransactionAsync(Transaction transaction) {
    _context.Transaction.Add(transaction);
    await _context.SaveChangesAsync();
    return transaction;
  }

  public async Task<bool> DeleteTransactionAsync(int id) {
    var transaction = await _context.Transaction.FindAsync(id);
    if (transaction == null) return false;

    _context.Transaction.Remove(transaction);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> TransactionExistsAsync(int id) {
    return await _context.Transaction.AnyAsync(t => t.Id == id);
  }

  // Asynchronicznie przelicza kwotę z jednej waluty na drugą, korzystając z CurrencyConverterService.
  public async Task<decimal> ConvertAsync(decimal amount, string from, string to) {
    return await _converter.ConvertAsync(amount, from, to);
  }
}