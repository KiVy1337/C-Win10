using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Exceptions;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Services.AuthenticationServices {
	public class AuthenticationService : IAuthenticationService {
		private readonly IAccountService _accountService;
		private readonly IPasswordHasher _passwordHasher;

		public AuthenticationService(IAccountService accountService, IPasswordHasher passwordHasher) {
			_accountService = accountService;
			_passwordHasher = passwordHasher;
		}
		//This method use methods from AccountService to get Account that user require or raise custom exception
		public async Task<Account> Login(string username, string password) {
			Account storedAccount = await _accountService.GetByUsername(username);

			if(storedAccount == null) {
				throw new UserNotFoundException(username);
			}

			PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedAccount.PasswordHash, password);
			if(passwordResult != PasswordVerificationResult.Success) {
				throw new InvalidPasswordException(username, password);
			}
			return storedAccount;

		}

		//This method use methods from AccountService to get information about new users data and to create a new Account
		public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword) {
			RegistrationResult result = RegistrationResult.Success;

			if(password != confirmPassword) {
				result = RegistrationResult.PasswordsDoNotMatch;
			}

			Account emailAccount = await _accountService.GetByEmail(email);

			if(emailAccount != null) {
				result = RegistrationResult.EmailAlreadyExists;
			}


			Account usernameAccount = await _accountService.GetByUsername(username);

			if (usernameAccount != null) {
				result = RegistrationResult.UsernameAlreadyExists;
			}

			if (result == RegistrationResult.Success) {
				string hasedPassword = _passwordHasher.HashPassword(password);

				Account account = new Account() {
					Email = email,
					Username = username,
					PasswordHash = hasedPassword,
					DatesJoined = DateTime.Today
				};

				await _accountService.Create(account);
			}

			return result;
		}
	}
}
