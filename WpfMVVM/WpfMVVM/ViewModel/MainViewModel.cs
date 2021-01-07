using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using WpfMVVM.Models;

namespace WpfMVVM.ViewModel {
	class MainViewModel : INotifyPropertyChanged {

        public IRepository db;
        private Employee selectedEmployee;
        private ObservableCollection<Employee> employeesList;
        private int loadbarvalue;
        private string loadbarcaption;
        private string loadbarvisibility;
        private string interruptionbtnvisibility;
        private string framevisibility;
        private string whichpage;
        private Dispatcher disp;
        private int numberofrecords;
        private CancellationTokenSource cancelTokenSource;
        private bool isload;

        //Commands
        private RelayCommand loadinterruptionCommand;
        private RelayCommand showupdatepageCommand;
        private RelayCommand showaddpageCommand;
        private RelayCommand deleteCommand;
        private RelayCommand exportCommand;


        public Employee SelectedEmployee {
            get { return selectedEmployee; }
            set {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public ObservableCollection<Employee> EmployeesList {
            get { return employeesList; }
            set {
                employeesList = value;
                OnPropertyChanged("EmployeesList");
            }
        }

        public int LoadbarValue {
            get { return loadbarvalue; }
            set {
                loadbarvalue = value;
                OnPropertyChanged("LoadbarValue");
            }
        }
        public string LoadbarCaption {
            get { return loadbarcaption; }
            set {
                loadbarcaption = value;
                OnPropertyChanged("LoadbarCaption");
            }
        }

        public string LoadBarVisibility {
			get { return loadbarvisibility; }
			set {
                loadbarvisibility = value;
                OnPropertyChanged("LoadBarVisibility");
            }
		}
        public string FrameVisibility {
            get { return framevisibility; }
            set {
                framevisibility = value;
                OnPropertyChanged("FrameVisibility");
            }
        }
        public string WhichPage {
            get { return whichpage; }
            set {
                whichpage = value;
                OnPropertyChanged("WhichPage");
            }
        }
        public string InterruptionBtnVisibility {
            get { return interruptionbtnvisibility; }
            set {
                interruptionbtnvisibility = value;
                OnPropertyChanged("InterruptionBtnVisibility");
            }
        }


        public RelayCommand ShowUpdateCommand {
            get {
                return showupdatepageCommand ??
                  (showupdatepageCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      Messenger.Default.Send(emp, "ChangeEmployee");
                      WhichPage = "UpdatePage";
                      FrameVisibility = "Visible";
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
        public RelayCommand ShowAddCommand {
            get {
                return showaddpageCommand ??
                  (showaddpageCommand = new RelayCommand(obj => {
                      WhichPage = "AddPage";
                      FrameVisibility = "Visible";
                  }));
            }
        }

        public RelayCommand LoadInterruptionCommand {
            get {
                return loadinterruptionCommand ??
                  (loadinterruptionCommand = new RelayCommand(obj => {
                      cancelTokenSource.Cancel();
                  },
                  (obj) => {
                        return !cancelTokenSource.Token.IsCancellationRequested;
                  }));
            }
        }

        public RelayCommand DeleteCommand {
            get {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj => {
                      System.Collections.IList items = (System.Collections.IList)obj;
                      IEnumerable<Employee> collection = items.Cast<Employee>();
                      db.DeleteRange(collection);
                      db.Save();
                      EmployeesList = db.GetEmployeeList();
                      numberofrecords = EmployeesList.Count;
                      LoadbarCaption = String.Format("Доступное количество записей: " + numberofrecords.ToString());
                      SelectedEmployee = null;

                  },
                  (obj) => {
                      if (obj == null) {
                          return false;
                      }
                      else {
                          System.Collections.IList items = (System.Collections.IList)obj;
                          IEnumerable<Employee> collection = items.Cast<Employee>();
                          if (collection.Count() == 0)
                              return false;
                          else
                              return true;
                      }
                  }));
            }
        }

        public RelayCommand ExportCommand {
            get {
                return exportCommand ??
                  (exportCommand = new RelayCommand(obj => {
                      Messenger.Default.Send(EmployeesList, "GetEmployeeList");
                      WhichPage = "ExportPage";
                      FrameVisibility = "Visible";
                  },
                  (obj) => {
                      if (isload)
                          return true;
                      else
                          return false;
                  }
                  ));
            }
        }

        public MainViewModel() {
            db = new MSSQLEmployeeRepository(0);
            disp = Dispatcher.CurrentDispatcher;
            Messenger.Default.Register<Employee>(this, EmployeeForAddReceived, "AddingEmployee");
            Messenger.Default.Register<Employee>(this, UpdateEmployee, "UpdateEmployee");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));
            LoadBarVisibility = "Collapsed";
            InterruptionBtnVisibility = "Collapsed";
            FrameVisibility = "Collapsed";
            cancelTokenSource = new CancellationTokenSource();
            LoadDataFromCSV(@"..\..\Data.csv");
        }

		private void UpdateEmployee(Employee emp) {
            if (db.IsEmployeeInDB(emp)) {
                db.Update(emp);
                db.Save();
                SelectedEmployee = emp;
            }
			else {
                MessageBox.Show("Изменяемый объект был удалён","Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
            SelectedEmployee = null;
            FrameVisibility = "Collapsed";

        }

		private void EmployeeForAddReceived(Employee emp) {
            db.Create(emp);
            db.Save();
            EmployeesList = db.GetEmployeeList();
            numberofrecords++;
            LoadbarCaption = String.Format("Доступное количество записей: " + numberofrecords.ToString());
            FrameVisibility = "Collapsed";
            SelectedEmployee = emp;
        }

		private async void LoadDataFromCSV(string path) {
            LoadBarVisibility = "Visible";
            InterruptionBtnVisibility = "Visible";
            isload = false;
            CancellationToken token = cancelTokenSource.Token;
            await Task.Run(() => LoadDataFromCSVAsync(path,token)); // async method call
            if (cancelTokenSource.IsCancellationRequested) {
                LoadbarCaption = String.Format("Загрузка данных была прервана!\n" +
                    "Доступное количество записей: " + numberofrecords.ToString());
            }
			else {
                LoadbarCaption = String.Format("Загрузка данных завершена!\n" +
                    "Доступное количество записей: " + numberofrecords.ToString());
            }
            LoadBarVisibility = "Collapsed";
            InterruptionBtnVisibility = "Collapsed";
            isload = true;
        }

        public void LoadDataFromCSVAsync(string path, CancellationToken token) {
            using (StreamReader sr = new StreamReader(path, Encoding.Default)) {
                string line;
                int i = 1;
                List<Employee> collemp = new List<Employee>();
                while ((line = sr.ReadLine()) != null) {
                    if (token.IsCancellationRequested) {
                        break;
                    }
                    string[] fields = line.Split(';');
                    collemp.Add(new Employee(fields));
                    // every 100th iteration add 100 samples to context in main thread help with Dispatcher.Invoke
                    if (i % 100 == 0) {
                        disp.Invoke(() => db.CreateRange(collemp));
                        disp.Invoke(() => db.Save());
                        EmployeesList = db.GetEmployeeList();
                        LoadbarCaption = String.Format("Загрузка данных {0}/50000", i);
                        LoadbarValue++;
                        collemp.Clear();
                    }
                    i++;
                }
                disp.Invoke(() => db.CreateRange(collemp));
                disp.Invoke(() => db.Save());
                EmployeesList = db.GetEmployeeList();
                numberofrecords = i;
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
