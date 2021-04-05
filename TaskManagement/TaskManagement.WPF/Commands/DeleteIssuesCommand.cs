using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services;
using TaskManagement.EntityFramework.Services;
using TaskManagement.WPF.ViewModels;

namespace TaskManagement.WPF.Commands {
	// command that deletes selected issues
	class DeleteIssuesCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly IssuesViewModel _isVM;

		public DeleteIssuesCommand(IssuesViewModel isVM) {
			_isVM = isVM;
		}

		public bool CanExecute(object parameter) {
			if (parameter == null) {
				return false;
			}
			else {
				return true;
			}
		}

		public async void Execute(object parameter) {
			System.Collections.IList items = (System.Collections.IList)parameter;
			IEnumerable<Issue> collection = items.Cast<Issue>();
			IDataService<Issue> issueService = new IssueDataService();
			await issueService.DeleteRange(collection);
			_isVM.InnerIssuesNavigator.CurrentViewModel = null;
			_isVM.UpdateData();
		}
	}
}
