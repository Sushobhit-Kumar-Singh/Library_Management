using Library_Management_Application.Authorization;
using Library_Management_Application.Models;
using Library_Management_Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Reflection.Metadata.BlobBuilder;

namespace Library_Management_Application.Controllers
{
	public class HomeController : Controller
    {
        private readonly LibraryService _libraryService;
        private readonly BorrowerService _borrowerService;

        public HomeController(LibraryService libraryService, BorrowerService borrowerService)
        {
            _libraryService = libraryService;
            _borrowerService = borrowerService;
        }

		public async Task<IActionResult> Index(string searchTerm)
        {
            int? memberId = HttpContext.Session.GetInt32("MemberId");

            var allBooks = await _libraryService.GetAllBooksAsync();

            var model = new HomeViewModel
            {
                AllBooks = allBooks,
                memberId = memberId ?? 1047
            };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.AllBooks = model.AllBooks.Where(b =>
                            b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)||
                            b.Genre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            b.Isbn.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)||
                            b.PublicationYear.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)||
                            b.CopiesAvailable.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)||
                            b.TotalCopies.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)

                        ).ToList();
            }

            if(model.AllBooks.Any())
            {
                return View(model);

            }

            else
            {
                ViewBag.Message = "No Books Found";
                return View();
            }

        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
		[HttpPost]
        public async Task<IActionResult> BorrowBook(string isbn)
        {
            try
            {

				int? memberId = HttpContext.Session.GetInt32("MemberId") ?? 1047;

                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Invalid input for borrowing a book.";
                    return RedirectToAction(nameof(Index), new { memberId= memberId??1047 });
                }

                if (await _borrowerService.BorrowBookAsync(isbn))
                {
                    return RedirectToAction(nameof(Index), new { memberId =memberId?? 1047 });
                }

                ViewBag.ErrorMessage = "Failed to borrow the book";

                return RedirectToAction(nameof(Index), new { memberId =memberId?? 1047 });
            }

            catch (Exception)
            {
                ViewBag.ErrorMessage = "An Error Occured while processing the request";

                return View("Error");
            }

        }

		[ServiceFilter(typeof(CustomAuthorizationFilter))]
		[HttpPost]
        public async Task<IActionResult> ReturnBook(string isbn)
        {
            try
            {
                int? memberId = HttpContext.Session.GetInt32("MemberId")??1047;

                if (string.IsNullOrEmpty(isbn) || memberId <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid input for returning a book.";
                    return RedirectToAction(nameof(Index), new { memberId = memberId ?? 1047 });
                }

                if (await _borrowerService.ReturnBookAsync(isbn))
                {
                    return RedirectToAction(nameof(Index), new { memberId = memberId ?? 1047 });
                }

                ViewBag.ErrorMessage = "Failed to return the book";
                return RedirectToAction(nameof(Index), new { memberId = memberId??1047 });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An Error Occurred while processing the request";
                return View("Error");
            }

        }

		[ServiceFilter(typeof(CustomAuthorizationFilter))]
		public IActionResult Borrowed()
        {
            int? memberId = HttpContext.Session.GetInt32("MemberId") ?? 1047;
            try
            {
				var borrowedBooks = _borrowerService.GetBorrowedBooks().Result;
				return View(borrowedBooks);
			}

            catch(Exception ex)
            {
                Console.WriteLine($"Error in getting borrowed books:{ex.Message}");
                throw;
            }
           
        }

		[ServiceFilter(typeof(CustomAuthorizationFilter))]
		public IActionResult Returned()
        {
            int? memberId = HttpContext.Session.GetInt32("MemberId") ?? 1047; ;

            try
            {
                var returnedBooks = _borrowerService.GetReturnedBooks().Result;
                return View(returnedBooks);
            }
            catch (Exception ex)
            {
                return View("Error:",ex.Message);
            }
        }

		[ServiceFilter(typeof(CustomAuthorizationFilter))]
		public async Task<IActionResult> Transactions()
		{
            int? memberId = HttpContext.Session.GetInt32("MemberId")??1047;

            var transactions = await _borrowerService.GetTransactions();
			return View(transactions);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
