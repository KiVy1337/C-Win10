using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagement.WPF.State.Authenticators;
using TaskManagement.WPF.State.Navigators;
using TaskManagement.WPF.ViewModels;

namespace TaskManagement.WPF.Commands {
	// command for logout
	class LogoutCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly MainViewModel _mainVM;


		public LogoutCommand(MainViewModel mainVM) {
			_mainVM = mainVM;
		}

		public bool CanExecute(object parameter) {
			if (_mainVM.Authenticator.IsLoggedIn == false){
				return false;
			}
			else {
				return true;
			}
		}

		public void Execute(object parameter) {
			_mainVM.Authenticator.Logout();
			_mainVM.UpdateCurrentViewModelCommand.Execute(ViewType.Login);

		}
	}
}
