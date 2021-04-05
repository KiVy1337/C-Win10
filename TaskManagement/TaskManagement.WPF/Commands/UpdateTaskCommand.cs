using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services;
using TaskManagement.EntityFramework.Services;
using TaskManagement.WPF.ViewModels;

namespace TaskManagement.WPF.Commands {
	class UpdateTaskCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly UpdateTaskViewModel _upVM;

		public UpdateTaskCommand(UpdateTaskViewModel upVM) {
			_upVM = upVM;
		}

		public bool CanExecute(object parameter) {
			if (_upVM.TaskToUpdate.Title != "" && _upVM.TaskToUpdate.Description != "") {
				return true;
			}
			else return false;
		}

		public async void Execute(object parameter) {
			IDataService<Task> taskService = new GenericDataService<Task>();
			IDataService<Issue> issueService = new IssueDataService();
			Task tas = _upVM.OriginalTask;
			Task tas2 = _upVM.TaskToUpdate;
			tas.Title = tas2.Title;
			tas.Description = tas2.Description;
			tas.Progress = tas2.Progress;

			await taskService.Update(tas);
			_upVM._navigator.ParentViewModel.SelectedIssue = await issueService.Get(tas.IssueId);
			_upVM.CloseView();
		}

	}
}
