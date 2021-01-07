using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfMVVM.Models;

namespace WpfMVVM.ViewModel {
	public class AddViewModel : INotifyPropertyChanged {
        private Employee employeetoadd;
        private RelayCommand addCommand;
        private RelayCommand clnaddpageCommand;

        public Employee EmployeeToAdd {
            get { return employeetoadd; }
            set {
                employeetoadd = value;
                OnPropertyChanged("EmployeeToAdd");
            }
        }

        public AddViewModel() {
            EmployeeToAdd = new Employee(DateTime.Today.ToString(), "", "", "", "", "");

        }

        public RelayCommand AddCommand {
            get {
                return addCommand ??
                  (addCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      Employee emptoadd = new Employee(emp);
                      Messenger.Default.Send(emptoadd, "AddingEmployee");
                  },
                  (obj) => {
                      Employee emp = obj as Employee;
                      if (emp == null)
                          return false;
                      else if (emp.Date == null || emp.FirstName == "" || emp.LastName == "" || emp.UserName == "" || emp.City == "" || emp.Country == "") {
                          return false;
                      }
                      else
                          return true;
                  }));
            }
        }

        public RelayCommand ClnAddPageCommand {
            get {
                return clnaddpageCommand ??
                  (clnaddpageCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      emp.Date = DateTime.Today;
                      emp.FirstName = "";
                      emp.LastName = "";
                      emp.UserName = "";
                      emp.City = "";
                      emp.Country = "";
                  },
                  (obj) => {
                      Employee emp = obj as Employee;
                      if (emp == null)
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
