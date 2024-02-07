using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Library_Management_Application.Authorization;
public class CustomAuthorizationFilter : IAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CustomAuthorizationFilter(IHttpContextAccessor httpContextAccessor)
    {

        _httpContextAccessor = httpContextAccessor;

    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var memberId= _httpContextAccessor.HttpContext?.Session.GetInt32("MemberId");
        if(!memberId.HasValue)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                
                {"controller","Auth" },
                {"action","SignIn" }

            });
            return;
        }
    }
}
