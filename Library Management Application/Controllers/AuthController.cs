using Library_Management_Application.Authorization;
using Library_Management_Application.Data;
using Library_Management_Application.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using static Library_Management_Application.Data.AuthService;

public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(AuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public IActionResult SignIn(Login model)
    {
        bool isApprovalPending;
        var signinResult = _authService.SignIn(model.EMail, model.Password, out isApprovalPending);

		model.IsApprovalPending = isApprovalPending;

		switch (signinResult)
        {
            case SigninResult.Librarian:
                return RedirectToAction("LibrarianDashboard", "Library");

            case SigninResult.BorrowerPendingApproval:
                TempData["IsApprovalPending"] = true;
                    return RedirectToAction("SignIn");

            case SigninResult.BorrowerApproved:
                return RedirectToAction("Index","Home");

            case SigninResult.BorrowerRejected:
                TempData["BorrowerRejected"] = true;
				return View("SignIn", model);

            case SigninResult.UserNotFound:
                TempData["UserNotFound"] = true;
                return View("SignIn", model);  
                
            case SigninResult.PasswordIncorrect:
                TempData["PasswordIncorrect"] = true;
                return View("SignIn", model);
        }

        return View("SignIn", model);   
	}

    public IActionResult SignIn()
    {
        if (TempData.TryGetValue("IsApprovalPending", out var isApprovalPending))
        {
            ViewData["IsApprovalPending"] = isApprovalPending;
            TempData.Remove("IsApprovalPending");
        }
        return View();
    }

    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public IActionResult LogoutConfirmed()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _httpContextAccessor.HttpContext?.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(Register model)
    {
        if (ModelState.IsValid)
        {
            _authService.SignUp(model.Name,model.Address,model.PhoneNumber,model.EMail, model.Password);

            TempData["SuccessfulSignup"] = true;

            return RedirectToAction("SignUpSuccess");
        }

        return View(model);
    }

    public IActionResult SignUpSuccess()
    {
		if (TempData.TryGetValue("SuccessfulSignup", out var successfulSignup))
        {
            ViewData["SuccessfulSignup"] = successfulSignup;
			TempData.Remove("SuccessfulSignup");
		}
		return View();
	}

    
}
