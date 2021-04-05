using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services.AuthenticationServices;

namespace TaskManagement.WPF.State.Authenticators {
	public interface IAuthenticator {
		Account CurrentAccount { get; }
		bool IsLoggedIn { get; }
		Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
		System.Threading.Tasks.Task Login(string username, string password);
		void Logout();  
	}
}
