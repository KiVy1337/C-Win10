using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services;
using TaskManagement.EntityFramework.Services;
using TaskManagement.WPF.Commands;
using TaskManagement.WPF.State.Authenticators;
using TaskManagement.WPF.State.Navigators;

namespace TaskManagement.WPF.ViewModels {
	public class IssuesViewModel : GenericViewModel<Issue> {
		private readonly IAuthenticator _authenticator;
		private readonly IDataService<Account> _accountService;

		private ICollection<Issue> _issues;
		private Issue _selectedItem;
		private string _buttonVisibility;

		public IInnerNavigator<Issue> InnerIssuesNavigator { get; set; }

		public ICommand DeleteIssuesCommand { get; set; }
		public ICommand UpdateStateTaskViewModelCommand { get; set; }

		public ICollection<Issue> Issues {
			get {
				return _issues;
			}
			set {
				_issues = value; 
				OnPropertyChanged(nameof(Issues));
			}
		}

		public TasksViewModel TasksVM { get; set; }

		public override Issue SelectedItem {
			get {
				return _selectedItem;
			}
			set {
				_selectedItem = value;
				if (value != null) {
					ButtonVisibility = "Visible";
				}
				else {
					ButtonVisibility = "Collapsed";
				}
				OnPropertyChanged(nameof(SelectedItem));
			}
		}
		public string ButtonVisibility {
			get {
				return _buttonVisibility;
			}
			set {
				_buttonVisibility = value;
				OnPropertyChanged(nameof(ButtonVisibility));
			}
		}

		public IssuesViewModel(IInnerNavigator<Issue> nav, IAuthenticator authenticator, IDataService<Account> accountService) {
			SelectedItem = null;
			InnerIssuesNavigator = nav;
			_authenticator = authenticator;
			_accountService = accountService;
			InnerIssuesNavigator.ParentViewModel = this;
			DeleteIssuesCommand = new DeleteIssuesCommand(this);
			if (authenticator.IsLoggedIn) {
				LoadIssues();
			}
		}
		public void LoadIssues() {
			_accountService.Get(_authenticator.CurrentAccount.Id).ContinueWith(task => {
				if (task.Exception == null) {
					Issues = task.Result.Issues;
				}
			});
		}

		public override async void UpdateData() {
			Account account = await _accountService.Get(_authenticator.CurrentAccount.Id);
			Issues = account.Issues;
			UpdateStateTaskViewModelCommand.Execute(null);
		}

	}
}
