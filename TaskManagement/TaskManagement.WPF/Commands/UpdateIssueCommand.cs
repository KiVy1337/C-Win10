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
	class UpdateIssueCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly UpdateIssueViewModel _upVM;

		public UpdateIssueCommand(UpdateIssueViewModel upVM) {
			_upVM = upVM;
		}

		public bool CanExecute(object parameter) {
			if (_upVM.IssueToUpdate.Title != "" && _upVM.IssueToUpdate.Status != "") {
				return true;
			}
			else return false;
		}

		public async void Execute(object parameter) {
			IDataService<Issue> issueService = new IssueDataService();
			Issue iss = _upVM.OriginIssue;
			Issue is2 = _upVM.IssueToUpdate;
			iss.Title = is2.Title;
			iss.StartDate = is2.StartDate;
			iss.Status = is2.Status;
			await issueService.Update(iss);
			_upVM.CloseView();
			_upVM._navigator.ParentViewModel.UpdateData();
		}

	}
}
