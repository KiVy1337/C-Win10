using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestTaskIBA {
	/// <summary>
	/// Логика взаимодействия для UpdateEmployeeWindow.xaml
	/// </summary>
	public partial class UpdateEmployeeWindow : Window {

		public UpdateEmployeeWindow(Employee emp) {
			InitializeComponent();
			DataContext = emp;
		}
		// method that check if texboxes are empty
		private void UpdateButton_Click(object sender, RoutedEventArgs e) {
			Employee ee = DataContext as Employee;
			if (DatePickerEmployee.Text == "") {
				MessageBox.Show("Введите дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				DatePickerEmployee.Focus();
			}
			else if (EmployeeFirstName.Text == "") {
				MessageBox.Show("Введите имя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				EmployeeFirstName.Focus();
			}
			else if (EmployeeLastName.Text == "") {
				MessageBox.Show("Введите фамилию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				EmployeeLastName.Focus();
			}
			else if (EmployeeUserName.Text == "") {
				MessageBox.Show("Введите 'Username'", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				EmployeeUserName.Focus();
			}
			else if (EmployeeCity.Text == "") {
				MessageBox.Show("Введите город", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				EmployeeCity.Focus();
			}
			else if (EmployeeCountry.Text == "") {
				MessageBox.Show("Заполните поле страна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				EmployeeCountry.Focus();
			}
			else {
				this.DialogResult = true;
			}
		}

	}
}
