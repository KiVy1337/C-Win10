using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMVVM.Models;

namespace WpfMVVM.ViewModel {
	public class UpdateViewModel : INotifyPropertyChanged {
        private Employee employeeforupdate;
        private RelayCommand applychangesCommand;

        public Employee EmployeeForUpdate {
            get { return employeeforupdate; }
            set {
                employeeforupdate = value;
                OnPropertyChanged("EmployeeForUpdate");
            }
        }
        public UpdateViewModel() {
            Messenger.Default.Register<Employee>(this, ChangeEmployee, "ChangeEmployee");
        }
        private void ChangeEmployee(Employee emp) {
            EmployeeForUpdate = emp;
		}

        public RelayCommand ApplyChangesCommand {
            get {
                return applychangesCommand ??
                  (applychangesCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      Messenger.Default.Send(EmployeeForUpdate, "UpdateEmployee");
                  },
                  (obj) => {
                      Employee emp = obj as Employee;
                      if (emp == null || emp.Date == null || emp.FirstName == "" || emp.LastName == "" || emp.UserName == "" || emp.City == "" || emp.Country == "")
                          return false;
                      else
                          return true;
                  }
                  ));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
