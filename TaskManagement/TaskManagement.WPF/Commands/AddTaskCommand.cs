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
	// Command that add new Task in DB
	class AddTaskCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly AddTaskViewModel _addVM;

		public AddTaskCommand(AddTaskViewModel addVM) {
			_addVM = addVM;
		}

		public bool CanExecute(object parameter) {
			if (_addVM.TaskToAdd.Title != "" && _addVM.TaskToAdd.Description != "") {
				return true;
			}
			else return false;
		}

		public async void Execute(object parameter) {
			IDataService<Task> tasksService = new GenericDataService<Task>();
			IDataService<Issue> issuesService = new IssueDataService();
			await tasksService.Create(_addVM.TaskToAdd);
			_addVM.navigator.ParentViewModel.SelectedIssue = await issuesService.Get(_addVM.TaskToAdd.IssueId);
			_addVM.CloseView();
		}
	}
}
