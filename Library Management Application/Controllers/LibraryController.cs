// LibraryController.cs

using Library_Management_Application.Authorization;
using Library_Management_Application.Data;
using Library_Management_Application.Models;
using Library_Management_Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library_Management_Application.Controllers
{

	public class LibraryController : Controller
    {
        private readonly LibraryService _libraryService;
        private readonly BorrowerService _borrowerService;
        private readonly AuthService _authService;

        public LibraryController(LibraryService libraryService, BorrowerService borrowerService,AuthService authService, IWebHostEnvironment webHostEnvironment)
        {
            _libraryService = libraryService;
            _borrowerService = borrowerService;
            _authService = authService;
        }

		[HttpGet]
        public async Task<IActionResult> LibrarianDashboard()
        {
            var books = await _libraryService.GetAllBooksAsync();
            return View(books);
        }

		public IActionResult SignUpApprovalList()
		{
			var unapprovedSignUps = _authService.GetUnapprovedSignUps();

			return View(unapprovedSignUps);
		}

        public IActionResult ApproveSignUp(int memberId)
        {
            _authService.ApprovedSignUps(memberId);

            TempData["ApprovalMessage"] = "The borrower sign up has been approved";

            return RedirectToAction("SignUpApprovalList");
        }

        public IActionResult RejectSignUp(int memberId)
        {
            _authService.RejectSignUps(memberId);

            TempData["RejectMessage"] = "The borrower sign up has been rejected";

            return RedirectToAction("SignUpApprovalList");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    

                   await _libraryService.CreateBookAsync(
                    book.Isbn,
                    book.Title,
                    book.Author,
                    book.Genre,
                    book.PublicationYear,
                    book.CopiesAvailable,
                    book.TotalCopies
                );
                    return RedirectToAction(nameof(LibrarianDashboard));
                }
                return View(book);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a book: {ex.Message}");
                throw;
            }


        }

        [HttpGet]
        public async Task<IActionResult> Delete(string isbn)
        {
            var book = await _libraryService.GetBookByIdAsync(isbn);
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string isbn)
        {
            try
            {
                await _libraryService.DeleteBookAsync(isbn);
                return RedirectToAction(nameof(LibrarianDashboard));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting a book: {ex.Message}");

                throw;
            }
        }


        public async Task<IActionResult> Details(string isbn)
        {
            var book = await _libraryService.GetBookByIdAsync(isbn);
            return View(book);
        }

        public async Task<IActionResult> Edit(string isbn)
        {
            var book = await _libraryService.GetBookByIdAsync(isbn);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                await _libraryService.UpdateBookAsync(
                                book.Isbn,
                                book.Title,
                                book.Author,
                                book.Genre,
                                book.PublicationYear,
                                book.CopiesAvailable,
                                book.TotalCopies
                            ); 
                           return RedirectToAction(nameof(LibrarianDashboard));
            }
            return View(book);
        }
    }
}
