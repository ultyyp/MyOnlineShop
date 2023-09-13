using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System.Data;

namespace OnlineShop.Domain.Services
{
    public class AccountService
    {
        private readonly IUnitOfWork _uow;
		private readonly IApplicationPasswordHasher _hasher;

		public AccountService(IUnitOfWork UOW, IApplicationPasswordHasher hasher)
        {
			_uow = UOW ?? throw new ArgumentNullException(nameof(UOW));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
		}


        public virtual async Task<Account> Register(string name, string email, string password, Role[] roles, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or whitespace.", nameof(password));

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

            return account;
        }

        public virtual async Task<Account> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
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
