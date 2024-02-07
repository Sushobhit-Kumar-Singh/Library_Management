using Library_Management_Application.Data;
using Library_Management_Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_Application.Service
{
    public class LibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(string isbn)
        {
            return await _context.Books.FindAsync(isbn);
        }

        public async Task<bool> CreateBookAsync(string isbn,string title,string author,string genre,short PublicatonYear,int copiesAvailable,int totalCopiesAvailable)
        {
            try
            {
                var book= new Book
                {
                    Isbn = isbn,
                    Title = title,
                    Author = author,
                    Genre = genre,
                    PublicationYear = PublicatonYear,
                    CopiesAvailable = copiesAvailable,
                    TotalCopies = totalCopiesAvailable,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsActive = true
                };
                   _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in creating book:{ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateBookAsync(string isbn, string title, string author, string genre, short PublicatonYear, int copiesAvailable, int totalCopiesAvailable)
        {
            try
            {
                var book = await _context.Books.FindAsync(isbn);
                if (book != null)
                {
                    book.Title = title;
                    book.Author = author;
                    book.Genre = genre;
                    book.PublicationYear = PublicatonYear;
                    book.CopiesAvailable = copiesAvailable;
                    book.TotalCopies = totalCopiesAvailable;
                    book.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(string isbn)
        {
            try
            {
                var transactions = await _context.Transactions.Where(t => t.BookIsbn == isbn).ToListAsync();
                _context.Transactions.RemoveRange(transactions);

                var book = await _context.Books.FindAsync(isbn);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a book: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Book>> SearchBooksAsync(string searchQuery)
        {
            return await _context.Books.Where(b => b.Title.Contains(searchQuery) || b.Author.Contains(searchQuery) || b.Genre.Contains(searchQuery)).ToListAsync();
        }
    }
}
