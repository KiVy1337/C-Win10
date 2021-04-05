using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;
using TaskManagement.WPF.State.Authenticators;
using TaskManagement.WPF.State.Navigators;

namespace TaskManagement.WPF.ViewModels.Factories {
	// Create new VM by ViewType
	class RootInnerViewModelFactory<T> : IRootInnerViewModelFactory<T> where T : DomainObject {
		private readonly IAuthenticator _authenticator;
		private IInnerNavigator<T> _navigator;

		public RootInnerViewModelFactory(IAuthenticator authenticator) {
			_authenticator = authenticator;
		}

		public IInnerNavigator<T> Navigator {
			get {
				return _navigator;
			}
			set {
				_navigator = value;
			}
		}

		public InnerViewModel CreateViewModel(ViewType viewType) {
			switch (viewType) {
				case ViewType.IssuesAdd:
					return new AddIssueViewModel(_navigator, _authenticator);
				case ViewType.IssuesUpdate:
					return new UpdateIssueViewModel(_navigator);
				case ViewType.TasksAdd:
					return new AddTaskViewModel(_navigator);
				case ViewType.TasksUpdate:
					return new UpdateTaskViewModel(_navigator);
				default:
					throw new ArgumentException("The ViewType doesn't have a ViewModel.", "viewType");
			}
		}
	}
}
