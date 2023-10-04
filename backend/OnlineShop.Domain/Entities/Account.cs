﻿using OnlineShop.Domain.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShop.Domain.Entities
{
    public class Account : IEntity
    {
        private string _name;
        private string _email;
        private string _hashedPassword;
		private Role[]? _roles;

		protected Account()
		{
		}

		public Account(Guid id, string name, string email, string hashedPassword, Role[] roles)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"'{nameof(email)}' cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException($"'{nameof(hashedPassword)}' cannot be null or whitespace.", nameof(hashedPassword));
            if(!new EmailAddressAttribute().IsValid(email))
                throw new ArgumentException("Value is not a valid email address.", nameof(email));

            Id = id;
            _name = name;
            _email = email;
            _hashedPassword = hashedPassword;
            _roles = roles;
        }

        public Guid Id { get; init; }

        public string Name { 
            get => _name;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace!", nameof(value));
                _name = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace!", nameof(value));
                if (!new EmailAddressAttribute().IsValid(value))
                    throw new ArgumentException("Value is not a valid email address.", nameof(value));
                _email = value;
            }
        }

        public string HashedPassword
        {
            get => _hashedPassword;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace!", nameof(value));
                _hashedPassword = value;
            }
        }

		public Role[] Roles
		{
			get => _roles;
			set => _roles = value ?? throw new ArgumentNullException(nameof(value));
		}

		public void GrantRole(Role role)
		{
			if (!Enum.IsDefined(typeof(Role), role))
				throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(Role));
			Roles = Roles.Append(role).ToArray();
		}
	}
}
