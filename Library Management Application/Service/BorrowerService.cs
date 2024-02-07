using Library_Management_Application.Data;
using Library_Management_Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_Application.Service;
public class BorrowerService
{
    private readonly LibraryContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BorrowerService(LibraryContext context,IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Transaction>> GetBorrowedBooks()
    {
        int? memberId = _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");

        if (memberId.HasValue)
        {
            return await _context.Transactions
                .Include(t => t.BookIsbnNavigation)
                .Where(t => t.MemberId == memberId && t.ReturnDate == null)
                .ToListAsync();
        }

        return new List<Transaction>();

    }

    public async Task<List<Transaction>> GetReturnedBooks()
    {
        int? memberId = _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");

        if (memberId.HasValue)
        {
            return await _context.Transactions
                .Include(t => t.BookIsbnNavigation)
                .Where(t => t.MemberId == memberId && t.ReturnDate != null)
                .ToListAsync();
        }

        return new List<Transaction>();
    } 
    public async Task<bool> BorrowBookAsync(string isbn)
    {
        try
        {
            int? memberId = _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");

            if(memberId.HasValue)
            {
                var member = await _context.Members.FindAsync(memberId);
                var book = await _context.Books.Include(b => b.Transactions).FirstOrDefaultAsync(b => b.Isbn == isbn);

                if (member != null && book != null && book.CopiesAvailable > 0)
                {
                    var transaction = new Transaction
                    {
                        BookIsbn = isbn,
                        MemberId = memberId,
                        IssueDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(14),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsActive = true
                    };

                    book.Transactions.Add(transaction);
                    book.CopiesAvailable--;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
        catch (Exception)
        { 
            return false;
        }
    }

    public async Task<bool> ReturnBookAsync(string isbn)
    {
        try
        {
            int? memberId = _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");


            if(memberId.HasValue)
            {
                var member = await _context.Members.FindAsync(memberId);

                var book = await _context.Books
                    .Include(b => b.Transactions)
                    .FirstOrDefaultAsync(b => b.Isbn == isbn);

                if (member != null && book != null)
                {
                    var transaction = book.Transactions.FirstOrDefault(t => t.MemberId == memberId && t.ReturnDate == null);

                    if (transaction != null)
                    {
                        book.CopiesAvailable++;
                        transaction.ReturnDate = DateTime.Now;
                        transaction.IsActive = false;

                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Transaction>> GetTransactions()
    {
        int? memberId = _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");

        if (memberId.HasValue)
        {
            return await _context.Transactions
                .Include(t => t.BookIsbnNavigation)
                .Where(t => t.MemberId == memberId)
                .ToListAsync();
        }

        return new List<Transaction>();
    }
}

