using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using OnlineShop.WebApi.Services;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
		private readonly ITokenService _tokenService;

		public AccountController(AccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
			_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }


        [HttpPost("register")]
        public async Task<ActionResult<LoginByCodeResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var customerRole = new Role[] { Role.Customer };
            await _accountService.Register(request.Name, request.Email, request.Password, customerRole, cancellationToken);
            var (account, codeId) = await _accountService.Login(request.Email, request.Password, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return new LoginByCodeResponse(account.Id, account.Name, token);
        }

        // 1-st stage of login
        [HttpPost("login")]
        public async Task<ActionResult<LoginByCodeResponse>> Login(
            LoginByPassRequest request,
            CancellationToken cancellationToken)
        {
            var (account, codeId) = await _accountService.Login(request.Email, request.Password, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return new LoginByCodeResponse(account.Id, account.Name, token);
        }

        // 2-nd stage of login
        [HttpPost("login_by_code")]
        public async Task<ActionResult<LoginByCodeResponse>> LoginByCode(
            LoginByCodeRequest request,
            CancellationToken cancellationToken)
        {
            var account = await _accountService.LoginByCode(request.Email, request.CodeId, request.Code, cancellationToken);
            var token = _tokenService.GenerateToken(account);
            return new LoginByCodeResponse(account.Id, account.Name, token);
        }


        [Authorize]
		[HttpGet("current")]
		public async Task<ActionResult<AccountResponse>> GetCurrentAccount(CancellationToken cancellationToken)
		{
			var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var guid = Guid.Parse(strId);
			var account = await _accountService.GetAccountById(guid, cancellationToken);
			return new AccountResponse(account.Id, account.Name, account.Email);
		}

		// [Authorize(Roles = nameof(Role.Admin))]
		[HttpGet("all")]
		public async Task<ActionResult<Account[]>> GetAllAccounts(CancellationToken cancellationToken)
		{
			try
			{
				var products = await _accountService.GetAll(cancellationToken);
				return Ok(products);
			}
			catch (ArgumentNullException e)
			{
				return Conflict(new ErrorResponse("Account not present!"));
			}
		}
	}
}
