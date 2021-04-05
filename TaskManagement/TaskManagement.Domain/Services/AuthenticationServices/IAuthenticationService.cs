using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Services.AuthenticationServices {
	// enum that contains different types of result
	public enum RegistrationResult {
		Success,
		PasswordsDoNotMatch,
		EmailAlreadyExists,
		UsernameAlreadyExists
	}
	public interface IAuthenticationService {
		Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
		Task<Account> Login(string username, string password);
	}
}
