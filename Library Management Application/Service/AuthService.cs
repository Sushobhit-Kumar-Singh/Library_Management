using Library_Management_Application.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Library_Management_Application.Data;
public class AuthService
{
    public readonly LibraryContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthService(LibraryContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public enum SigninResult
    {
        UserNotFound,
        Librarian,
        BorrowerPendingApproval,
        BorrowerApproved,
        BorrowerRejected,
        PasswordIncorrect
    }

    public bool IsLibrarian(string email)
    {
        var user = _context.Members.FirstOrDefault(m => m.Email.ToLower() == email.ToLower() && m.RoleType=="Librarian");

        return user != null;
    }

    public SigninResult SignIn(string email, string password, out bool isApprovalPending)
    {
        isApprovalPending = false;
        var user = _context.Members.FirstOrDefault(m => m.Email.ToLower() == email.ToLower());

        if (user != null)
        {
            if (IsLibrarian(email))
            {
                if (VerifyPasswordLibrarian(password, user.Password))
                {

                    _httpContextAccessor.HttpContext?.Session.SetInt32("MemberId", user.MemberId);
                    _httpContextAccessor.HttpContext?.Session.SetString("RoleType", user.RoleType);

                    return SigninResult.Librarian;
                }
                else
                {
                    return SigninResult.PasswordIncorrect;
                }
            }
            else
            {
                if (user.IsActive == false)
                {

                    isApprovalPending = true;
                    return SigninResult.BorrowerPendingApproval;
                }

                if (VerifyPassword(password, user.Password))
                {
                    if(user.IsActive??false)
                    {
                        _httpContextAccessor.HttpContext?.Session.SetInt32("MemberId", user.MemberId);
                        _httpContextAccessor.HttpContext?.Session.SetString("RoleType", user.RoleType);
                        _httpContextAccessor.HttpContext?.Session.SetString("MemberName", user.Name);


                        return SigninResult.BorrowerApproved;
                    }
                    else
                    {
                        return SigninResult.BorrowerRejected;
                    }
                }
                else
                {
                    return SigninResult.PasswordIncorrect;
                }
            }
        }

        return SigninResult.UserNotFound;
    }


    public bool SignUp(string ?name, string ?address, string ?phoneno, string email, string password)
    {
        var hashPassword = HashPassword(password);
        var newUser = new Member
        {
            RoleType = "Borrower",
            Name = name,
            Address = address,
            PhoneNumber = phoneno,
            Email = email,
            Password= hashPassword,
            IsActive = false
        };

        _context.Members.Add(newUser);
        _context.SaveChanges();

        _httpContextAccessor.HttpContext?.Session.SetInt32("MemberId", newUser.MemberId);
        _httpContextAccessor.HttpContext?.Session.SetString("RoleType", newUser.RoleType);

        return true;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }

    private bool VerifyPasswordLibrarian(string enteredPassword, string storedPassword)
    {
        return enteredPassword == storedPassword;
    }

    public List<Register> GetUnapprovedSignUps()
    {
        var unapprovedSignups = _context.Members
            .Where(m => m.RoleType == "Borrower" && m.IsActive == false)
            .Select(m=>new Register
            {
               MemberId=m.MemberId,
               Name=m.Name,
               Address=m.Address,
               PhoneNumber=m.PhoneNumber,
               EMail=m.Email,
               IsApproved=m.IsActive??false
            })
            .ToList();
        return unapprovedSignups;
    }

    public void ApprovedSignUps(int memberId)
    {
        var user = _context.Members.Find(memberId);

        if(user!=null && user.RoleType=="Borrower")
        {
            user.IsActive = true;
            _context.SaveChanges();
        }
    }

    public void RejectSignUps(int memberId)
    {
        var user = _context.Members.Find(memberId);

        if(user!=null && user.RoleType=="Borrower")
        {
            _context.Remove(user);
            _context.SaveChanges();
        }
    }
}
