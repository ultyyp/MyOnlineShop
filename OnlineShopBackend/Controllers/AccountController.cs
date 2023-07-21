using Microsoft.AspNetCore.Mvc;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    { 
        [HttpPost("register")]
        public async Task<IActionResult> Register(Account account, IAccountRepository accountRepository, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(accountRepository);
            await accountRepository.Add(account, cancellationToken);
            return Ok();
        }
        

    }

}
