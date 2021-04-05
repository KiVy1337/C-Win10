using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Services;
using TaskManagement.EntityFramework.Services;
using TaskManagement.WPF.Commands;
using TaskManagement.WPF.State.Navigators;

namespace TaskManagement.WPF.ViewModels {
	public class TasksViewModel : GenericViewModel<Task> {
		private readonly IDataService<Issue> _issueService;
		public IInnerNavigator<Task> InnerTasksNavigator { get; set; }

		private Issue _selectedIssue;
		private Task _selectedItem;

		private string _updateDeleteButtonsVisibility;
		private string _addButtonVisibility;

		public ICommand DeleteTasksCommand { get; set; }


		public override Issue SelectedIssue {
			get {
				return _selectedIssue;
			}
			set {
				Issue test = null;
				if (value != null) {
					AddButtonVisibility = "Visible";
					_issueService.Get(value.Id).ContinueWith(task => {
						if (task.Exception == null) {
							test = task.Result;
							_selectedIssue = test;
							OnPropertyChanged(nameof(SelectedIssue));
							OnPropertyChanged(nameof(Tasks));
						}
					});
				}
				else {
					AddButtonVisibility = "Collapsed";
					test = null;
				}
				_selectedIssue = test;
				SelectedItem = null;
				OnPropertyChanged(nameof(SelectedIssue));
				OnPropertyChanged(nameof(Tasks));
			}
		}

		public IEnumerable<Task> Tasks {
			get {
				if(SelectedIssue != null) {
					return SelectedIssue.Tasks;
				}
				return null;
			}
		}
		public string UpdateDeleteButtonsVisibility {
			get {
				return _updateDeleteButtonsVisibility;
			}
			set {
				_updateDeleteButtonsVisibility = value;
				OnPropertyChanged(nameof(UpdateDeleteButtonsVisibility));
			}
		}

		public string AddButtonVisibility {
			get {
				return _addButtonVisibility;
			}
			set {
				_addButtonVisibility = value;
				OnPropertyChanged(nameof(AddButtonVisibility));
			}
		}
		public override Task SelectedItem {
			get {
				return _selectedItem;
			}
			set {
				_selectedItem = value;
				if (value != null) {
					UpdateDeleteButtonsVisibility = "Visible";
				}
				else {
					UpdateDeleteButtonsVisibility = "Collapsed";
				}
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		public TasksViewModel(IInnerNavigator<Task> navigator, IDataService<Issue> issueService) {
			InnerTasksNavigator = navigator;
			_issueService = issueService;
			InnerTasksNavigator.ParentViewModel = this;
			UpdateDeleteButtonsVisibility = "Collapsed";
			AddButtonVisibility = "Collapsed";
			DeleteTasksCommand = new DeleteTasksCommand(this);
		}

	}
}
