using MediatR;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System.Data;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _uow;  
		private readonly IApplicationPasswordHasher _hasher;
        private readonly IMediator _mediator;


        public AccountService(IUnitOfWork UOW, IApplicationPasswordHasher hasher, IMediator mediator)
        {
			_uow = UOW ?? throw new ArgumentNullException(nameof(UOW));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
             _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}


        public virtual async Task Register(string name, string email, string password, Role[] roles, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));

            var accountExists = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);

            if (accountExists is not null)
            {
                throw new EmailAlreadyExistsException("Account with given email already exists.");
			}

			Account account = new Account(Guid.NewGuid(), name, email, EncryptPassword(password), roles);
			Cart cart = new(Guid.NewGuid(), account.Id);
			await _uow.AccountRepository.Add(account, cancellationToken);
			await _uow.CartRepository.Add(cart, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

            //Signalises that acount has registered
            await _mediator.Publish(new AccountRegistered(account, DateTime.UtcNow), cancellationToken);
        }


        public virtual async Task<(Account account, Guid codeId)> Login(string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var account = await LoginByPassword(email, password, cancellationToken);

            var code = await CreateAndSendConfirmationCode(account, cancellationToken);

            return (account, code.Id);
        }

        public async Task<Account> LoginByCode(string email, Guid codeId, string code, CancellationToken cancellationToken)
        {

            var codeObject = await _uow.ConfirmationCodeRepository.GetById(codeId, cancellationToken);
            if (codeObject is null)
                throw new CodeNotFoundException("There is no Code for this CodeId");
            if (codeObject.Code != code)
                throw new InvalidCodeException("Code not confirmed!");
            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
                throw new AccountNotFoundException("Account not found");
            return account;
        }

        private async Task<ConfirmationCode> CreateAndSendConfirmationCode(Account account, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (account.Email == null) throw new ArgumentNullException(nameof(account));
            var code = GeneraNewConfirmationCode(account);
            await _uow.ConfirmationCodeRepository.Add(code, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            //Singnalises to send login code
            await _mediator.Publish(new SendLogin2FA(account, code), cancellationToken);

            return code;
        }

        private ConfirmationCode GeneraNewConfirmationCode(Account account)
        {
            return new ConfirmationCode(Guid.NewGuid(), account.Id, DateTimeOffset.Now,
                DateTimeOffset.Now);
        }

        public virtual async Task<Account> LoginByPassword(string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
            {
                throw new AccountNotFoundException("Account with given email not found");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            if (rehashNeeded)
            {
                await RehashPassword(password, account, cancellationToken);
            }

            return account;
        }



        private Task RehashPassword(string password, Account account, CancellationToken cancellationToken)
		{
			account.HashedPassword = EncryptPassword(password);
            _uow.AccountRepository.Update(account, cancellationToken);
            _uow.SaveChangesAsync();
            return Task.CompletedTask;
		}

		private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            return hashedPassword;
        }

        public async Task<Account> GetAccountById(Guid guid, CancellationToken cancellationToken)
        {
            return await _uow.AccountRepository.GetById(guid, cancellationToken);
        }

		public async Task<IReadOnlyList<Account>>GetAll(CancellationToken cancellationToken)
		{
			return await _uow.AccountRepository.GetAll(cancellationToken);
		}
	}
}
