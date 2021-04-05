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
	// Command that addы new Issue in DB
	class AddIssueCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly AddIssueViewModel _addVM;

		public AddIssueCommand(AddIssueViewModel addVM) {
			_addVM = addVM;
		}

		public bool CanExecute(object parameter) {
			if (_addVM.IssueToAdd.Title != "" && _addVM.IssueToAdd.Status != "") {
				return true;
			}
			else return false;
		}

		public async void Execute(object parameter) {
			IDataService<Issue> issuesService = new IssueDataService();
			await issuesService.Create(_addVM.IssueToAdd);
			_addVM.CloseView();
			_addVM.navigator.ParentViewModel.UpdateData();
		}
	}
}
