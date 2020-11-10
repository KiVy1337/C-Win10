using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestTaskIBA {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public EmployeesDBContext context;
		public MainWindow() {
			InitializeComponent();
			LoadDataFromCSV(@"..\..\Data.csv");
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));
			this.Closing += MainWindow_Closing;
		}

		//Method of closing main window
		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			context.Dispose();
		}

		//the method that creates context and call the async method that load the data from csv file
		private async void LoadDataFromCSV(string path) {
			context = new EmployeesDBContext(0);
			EmployeesGrid.ItemsSource = context.Employees.Local.ToBindingList();
			await Task.Run(() => LoadDataFromCSVAsync(path)); // async method call
			MessageBox.Show("Загружены все данные", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			LoadBarTextBlock.Text = "Все данные загружены";
			Export_Button.IsEnabled = true;

		}

		//async method that load data
		public void LoadDataFromCSVAsync(string path) {
			using (StreamReader sr = new StreamReader(path, Encoding.Default)) {
				string line;
				int i = 1;
				List<Employee> collemp = new List<Employee>();
				while ((line = sr.ReadLine()) != null) {
					string[] fields = line.Split(';');
					collemp.Add(new Employee(fields));
					// every 100th iteration add 100 samples to context in main thread help with Dispatcher.Invoke
					if (i % 100 == 0) {
						Dispatcher.Invoke(() => context.Employees.AddRange(collemp));
						Dispatcher.Invoke(() => context.SaveChanges());
						Dispatcher.Invoke(() => LoadBar.Value++);
						Dispatcher.Invoke(() => LoadBarTextBlock.Text = String.Format("Загрузка данных {0}/50000", i));
						collemp.Clear();
					}
					i++;
				}
				Dispatcher.Invoke(() => context.SaveChanges());
			}

	    }

		//the method that create window wherein you can write data about employee which you wanna add
		private void Add_Button_Click(object sender, RoutedEventArgs e) {
			AddEmployeeWindow AddWindow = new AddEmployeeWindow(); // creating window
			AddWindow.Owner = this;
			if (AddWindow.ShowDialog() == true) {
				string[] fields = { 
					AddWindow.DatePickerAddEmployee.SelectedDate.ToString(),
					AddWindow.EmployeeFirstName.Text,
					AddWindow.EmployeeLastName.Text,
					AddWindow.EmployeeUserName.Text,
					AddWindow.EmployeeCity.Text,
					AddWindow.EmployeeCountry.Text

				};
				context.Employees.Add(new Employee(fields)); // add employee to the context
				EmployeesGrid.SelectedItem = EmployeesGrid.Items[EmployeesGrid.Items.Count - 1];
				EmployeesGrid.ScrollIntoView(EmployeesGrid.Items[EmployeesGrid.Items.Count - 1]);

			}
			else MessageBox.Show("Добавление отменено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);

		}

		//the method that delete selected samples from DB
		private void Delete_Button_Click(object sender, RoutedEventArgs e) {
			int cnt =  EmployeesGrid.SelectedItems.Count;
			if (cnt == 0) {
				MessageBox.Show("Удаление отменено.Выберите элементы для удаление.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else {
				List<Employee> l = new List<Employee>(cnt);
				for (int i = 0; i < cnt; i++) {
					l.Add(EmployeesGrid.SelectedItems[i] as Employee);
				}
				context.Employees.RemoveRange(l);
			}
			context.SaveChanges();
		}

		//the method that create window wherein you can update data about selected employee
		private void Update_Button_Click(object sender, RoutedEventArgs e) {
			if (EmployeesGrid.SelectedItem == null) {
				MessageBox.Show("Выберите сотрудника для обновления.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else {
				Employee emp = EmployeesGrid.SelectedItem as Employee;
				UpdateEmployeeWindow UpdateWindow = new UpdateEmployeeWindow(emp);
				UpdateWindow.Owner = this;
				UpdateWindow.ShowDialog();
				context.SaveChanges();
			}
		}

		// the method that create window wherein you can export data to XML or Excel data
		private void Export_Button_Click(object sender, RoutedEventArgs e) {
			ExportingDataWindow expwindow = new ExportingDataWindow(context);
			expwindow.Owner = this;
			expwindow.Show();
		}
	}
}
