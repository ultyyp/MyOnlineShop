using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
		private readonly IApplicationPasswordHasher _hasher;

		public AccountService(IAccountRepository accountRepository, IApplicationPasswordHasher hasher)
        {
			_accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
		}


        public async Task<Account> Register(string name, string email, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));

            var accountExists = await _accountRepository.FindAccountByEmail(email, cancellationToken);

            if (accountExists is not null)
            {
                throw new EmailAlreadyExistsException("Account with given email already exists.");
            }

            var account = new Account(Guid.Empty, name, email, EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);
            return account;
            
        }

        public async Task<Account> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if(account is null)
            {
                throw new AccountNotFoundException("Account with given email not found.");
            }


            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if(!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid Password.");
            }

            if(rehashNeeded)
			{
				await RehashPassword(password, account, cancellationToken);
			}

            return account;
		}

		private Task RehashPassword(string password, Account account, CancellationToken cancellationToken)
		{
			account.HashedPassword = EncryptPassword(password);
            return _accountRepository.Update(account, cancellationToken);
		}

		private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            return hashedPassword;
        }
    }
}
