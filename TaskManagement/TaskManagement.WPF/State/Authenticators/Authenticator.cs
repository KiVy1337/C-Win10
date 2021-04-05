using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services.AuthenticationServices;
using TaskManagement.WPF.Models;

namespace TaskManagement.WPF.State.Authenticators {
	//class that contains current account and can login and register help with IAuthenticationService 
	public class Authenticator : ObservableObject, IAuthenticator {
		private readonly IAuthenticationService _authenticationService;
		private Account _currentAccount;

		public Authenticator(IAuthenticationService authenticationService) {
			_authenticationService = authenticationService;
		}

		public Account CurrentAccount {
			get {
				return _currentAccount;
			}
			private set {
				_currentAccount = value;
				OnPropertyChanged(nameof(CurrentAccount));
				OnPropertyChanged(nameof(IsLoggedIn));
			}
		}

		public bool IsLoggedIn => CurrentAccount != null;

		public async System.Threading.Tasks.Task Login(string username, string password) {

			CurrentAccount = await _authenticationService.Login(username, password);
		}

		public void Logout() {
			CurrentAccount = null;
		}

		public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword) {
			return await _authenticationService.Register(email, username, password, confirmPassword);
		}
	}
}
