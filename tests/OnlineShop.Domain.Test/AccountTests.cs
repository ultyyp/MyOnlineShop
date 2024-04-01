using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Events.Handlers;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using Xunit;

namespace OnlineShop.Domain.Test
{
    public class AccountTests
    {
        private readonly Mock<ILogger<AccountService>> _loggerMock = new Mock<ILogger<AccountService>>();
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        private async Task<AccountService> SetupAccountServiceAsync()
        {
            var accountRepMock = new Mock<IAccountRepository>();
            var confCodeRepMock = new Mock<IConfirmationCodeRepository>();

            accountRepMock.Setup(x => x.Add(It.IsAny<Account>(), default))
                .Callback<Account, CancellationToken>((acc, ct) => { });

            var cartRepMock = new Mock<ICartRepository>();
            cartRepMock.Setup(x => x.Add(It.IsAny<Cart>(), default))
                .Callback<Cart, CancellationToken>((cart, ct) => { });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
            uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);

            var hasher = new Mock<IApplicationPasswordHasher>();
            hasher.Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns<string>(pass => pass + "_hashed");

            return new AccountService(uowMock.Object, hasher.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Account_Registered_Event_Notifies_User_By_Email()
        {
            // Arrange
            var user = new MockUser
            {
                Name = "John",
                Email = "John@john.com",
                Password = "qwerty",
                Roles = new[] { Role.Customer }
            };
            var account = new Account(Guid.NewGuid(), user.Name, user.Email, user.Password, user.Roles);
            var loggerMock = new Mock<ILogger<AccountRegisteredHandler>>();
            var emailSenderMock = new Mock<IEmailSender>();
            var handler = new AccountRegisteredHandler(emailSenderMock.Object, loggerMock.Object);
            var @event = new AccountRegistered(account, DateTime.Now);

            // Act
            await handler.Handle(@event, default);

            // Assert
            // 1. Activation of the trigger
            handler.Should().BeAssignableTo<INotificationHandler<AccountRegistered>>();

            // 2. Check that the email was sent
            emailSenderMock.Verify(it =>
                it.SendEmailAsync(account.Email, It.IsAny<string>(), It.IsAny<string>(), default), Times.Once);
        }

        [Theory]
        [InlineData(null, "password")]
        [InlineData("user", null)]
        public async Task Account_Registration_Throws_Argument_Null_Exception(string email, string password)
        {
            // Arrange
            var service = await SetupAccountServiceAsync();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Register("name", email, password, null, default));
        }

        [Fact]
        public async Task New_Account_Registered()
        {
            // Arrange
            var userGenerator = new Faker<MockUser>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Roles, () => new[] { Role.Customer });
            var user = userGenerator.Generate();

            var service = await SetupAccountServiceAsync();
            var createdAccounts = new List<Account>(); // Track created accounts

            // Act
            await service.Register(user.Name, user.Email, user.Password, user.Roles, default);

            // Assert
            // Check that the handler was called
            _mediatorMock.Verify(it => it.Publish(It.IsAny<AccountRegistered>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Second_account_with_same_email_registration_throws_exception()
        {
            // Arrange
            var testAccounts = new List<Account>();
            var testCart = new List<Cart>();

            var accountRepMock = new Mock<IAccountRepository>();
            accountRepMock
                .Setup(x => x.FindAccountByEmail(It.IsAny<string>(), default))
                .ReturnsAsync((string email, CancellationToken ct) =>
                    testAccounts.FirstOrDefault(u => u.Email == email));

            accountRepMock
                .Setup(x => x.Add(It.IsAny<Account>(), default))
                .Callback<Account, CancellationToken>((acc, ct) => testAccounts.Add(acc));

            var cartRepMock = new Mock<ICartRepository>();
            cartRepMock
                .Setup(x => x.Add(It.IsAny<Cart>(), default))
                .Callback<Cart, CancellationToken>((cart, ct) => testCart.Add(cart));

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.AccountRepository).Returns(accountRepMock.Object);
            uowMock.Setup(uow => uow.CartRepository).Returns(cartRepMock.Object);

            var hasher = new Mock<IApplicationPasswordHasher>();
            hasher
                .Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns<string>(pass => pass + "_hashed");

            var userGenerator = new Faker<MockUser>()
                .RuleFor(u => u.Name, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Roles, () => new[] { Role.Customer });
            var user = userGenerator.Generate();

            var loggerMock = new Mock<ILogger<AccountService>>();
            var mediatorMock = new Mock<IMediator>();

            var service = new AccountService(uowMock.Object, hasher.Object, mediatorMock.Object);

            // Act & Assert
            await service.Register(user.Name, user.Email, user.Password, user.Roles, default);

            Func<Task> act1 = () => service.Register(user.Name, user.Email, user.Password, user.Roles, default);

            await act1.Should().ThrowAsync<EmailAlreadyExistsException>();
        }


        public class MockUser
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public Role[] Roles { get; set; }
        }
    }
}
