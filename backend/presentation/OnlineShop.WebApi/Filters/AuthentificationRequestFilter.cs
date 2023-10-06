using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShop.WebApi.Filters;

public class AuthentificationRequestFilter : Attribute, IAuthorizationFilter
{
    private readonly ILogger<AuthentificationRequestFilter> _logger;

    public AuthentificationRequestFilter(
        ILogger<AuthentificationRequestFilter> logger
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers["Api-Key"].ToString();

        if (string.IsNullOrEmpty(token) || token != "OnlineShopApiToken")
        {
            _logger.LogInformation("Wrong API-KEY during Authorization");
            context.Result = 
                new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }
        _logger.LogInformation("Success confirmation of API-KEY");
    }
}