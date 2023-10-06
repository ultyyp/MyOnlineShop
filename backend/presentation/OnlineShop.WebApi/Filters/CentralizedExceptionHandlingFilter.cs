using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShop.Domain.Exceptions;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Filters;

public class CentralizedExceptionHandlingFilter 
    : Attribute, IExceptionFilter, IOrderedFilter
{
    public int Order { get; set; }

    public void OnException(ExceptionContext context)
    {
        var message = TryGetUserMessageFromException(context);
        HttpStatusCode statusCode = HttpStatusCode.Conflict;
        if (message != null)
        {
            context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
            {
                StatusCode = (409)
            };
            context.ExceptionHandled = true;
        }
    }

    private string? TryGetUserMessageFromException(ExceptionContext context)
    {
        return context.Exception switch
        {
            AccountNotFoundException => "Account With That Email Not Found!",
            EmailAlreadyExistsException => "Account With That Email Already Registered!",
            InvalidPasswordException => "Incorrrect Password",
            CodeNotFoundException => "Code Not Generated For That Account!",
            InvalidCodeException => "Invalid Code!",
            DomainException => "Unhandled Exception!",
            _ => null
        };
    }
    
}