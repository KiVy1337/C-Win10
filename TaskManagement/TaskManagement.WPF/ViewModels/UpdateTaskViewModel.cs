using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TaskManagement.Domain.Models;
using TaskManagement.WPF.Commands;
using TaskManagement.WPF.State.Navigators;

namespace TaskManagement.WPF.ViewModels {
	class UpdateTaskViewModel : InnerViewModel {
		private IEnumerable<string> _statusList;
		private Task _taskToUpdate;
		private Task _originalTask;

		public IInnerNavigator<Task> _navigator;

		public ICommand UpdateTaskCommand { get; set; }
		public ICommand CloseInnerViewCommand { get; set; }

		public Task TaskToUpdate {
			get {
				return _taskToUpdate;
			}
			set {
				_taskToUpdate = value;
				OnPropertyChanged(nameof(TaskToUpdate));
			}
		}
		public Task OriginalTask {
			get {
				return _originalTask;
			}
			set {
				_originalTask = value;
				OnPropertyChanged(nameof(OriginalTask));
			}
		}

		public UpdateTaskViewModel(INavigator<InnerViewModel> navigator) {
			_navigator = (IInnerNavigator<Task>)navigator;
			OriginalTask = _navigator.ParentViewModel.SelectedItem;
			TaskToUpdate = new Task() {
				Id = OriginalTask.Id,
				IssueId = OriginalTask.IssueId,
				Title = OriginalTask.Title,
				Description = OriginalTask.Description,
				Progress = OriginalTask.Progress
			};
			UpdateTaskCommand = new UpdateTaskCommand(this);
			CloseInnerViewCommand = new CloseInnerViewCommand(this);
		}
		public override void CloseView() {
			_navigator.CurrentViewModel = null;
		}
	}
}
