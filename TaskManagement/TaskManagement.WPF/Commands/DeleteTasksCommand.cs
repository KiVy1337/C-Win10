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
	// command that deletes selected tasks
	class DeleteTasksCommand : ICommand {
		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		private readonly TasksViewModel _isVM;

		public DeleteTasksCommand(TasksViewModel isVM) {
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
			IEnumerable<Task> collection = items.Cast<Task>();
			//IEnumerable<Issue> issues = (IEnumerable<Issue>)parameter;
			IDataService<Task> taskService = new GenericDataService<Task>();
			IDataService<Issue> issueService = new IssueDataService();
			await taskService.DeleteRange(collection);
			_isVM.InnerTasksNavigator.CurrentViewModel = null;
			_isVM.SelectedIssue = await issueService.Get(_isVM.SelectedIssue.Id);
		}
	}
}
