using LinqKit;
using Microsoft.Win32;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
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
using System.Xml.Linq;

namespace TestTaskIBA {
	/// <summary>
	/// Логика взаимодействия для ExportingDataWindow.xaml
	/// </summary>
	/// 
	// in this class there are 2 fields: collections that conteins all data and collection that contains data that will be exported
	public partial class ExportingDataWindow : Window {

		public ObservableCollection<Employee> ExportEmployees;
		public ObservableCollection<Employee> OriginalEmployees;
		public ExportingDataWindow(EmployeesDBContext context) {
			InitializeComponent();
			ExportEmployees = context.Employees.Local;
			OriginalEmployees = context.Employees.Local;
			EmployeesGrid.ItemsSource = ExportEmployees.ToBindingList();

		}

		//the method that call method that exports data to xml
		private async void XML_Export_Button_Click(object sender, RoutedEventArgs e) {
			if (ExportEmployees.Count == 0) {
				MessageBox.Show("Нечего экспортировать, выберите данные.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else {
				XML_Export_Button.IsEnabled = false;
				await Task.Run(() => XML_ExportAsync(ExportEmployees)); // call async method
				XML_Export_Button.IsEnabled = true;
			}
		}

		// method that export data to xml
		private void XML_ExportAsync(ObservableCollection<Employee> export_employees) {
			XDocument xdoc = new XDocument();
			XElement xrecords = new XElement("Records"); // creating a root of xml document 

			// creating elements of xml document and filling int with data from exported collection
			foreach (Employee emp in export_employees) {
				XElement xrecord = new XElement("Record");
				XAttribute xid = new XAttribute("id", emp.Id);
				XElement xdate = new XElement("Date", emp.Date.Date);
				XElement xfirstname = new XElement("FirstName", emp.FirstName);
				XElement xlastname = new XElement("LastName", emp.LastName);
				XElement xusername = new XElement("UserName", emp.UserName);
				XElement xcity = new XElement("City", emp.City);
				XElement xcountry = new XElement("Country", emp.Country);

				xrecord.Add(xid, xdate, xfirstname, xlastname, xusername, xcity, xcountry); // add elements to one 
				xrecords.Add(xrecord); //add to root
			}

			xdoc.Add(xrecords); // add root to document

			// save to file or create new file 
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = "c:\\";
			saveFileDialog.Filter = "XML files(*.xml)|*.xml";
			saveFileDialog.DefaultExt = ".xml";
			if (saveFileDialog.ShowDialog() == false)
				MessageBox.Show("Экспорт отменён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			else {
				xdoc.Save(saveFileDialog.FileName);
			}
		}

		//the method that call method that exports data to excel
		private async void Excel_Export_Button_Click(object sender, RoutedEventArgs e) {
			if (ExportEmployees.Count == 0) {
				MessageBox.Show("Нечего экспортировать, выберите данные.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else {
				Excel_Export_Button.IsEnabled = false;
				await Task.Run(() => Excel_ExportAsync(ExportEmployees));
				Excel_Export_Button.IsEnabled = true;
			}

		}
		// method that export data to excel
		private void Excel_ExportAsync(ObservableCollection<Employee> CopyExportEmployees) {
			using (ExcelEngine excelEngine = new ExcelEngine()) {
				IApplication application = excelEngine.Excel;
				application.DefaultVersion = ExcelVersion.Excel2016;

				//Create a new workbook
				IWorkbook workbook = application.Workbooks.Create(1);
				IWorksheet sheet = workbook.Worksheets[0];

				//Import data from exportcollection
				sheet.ImportData(CopyExportEmployees, 3, 1, false);

				IStyle pageHeader = workbook.Styles.Add("PageHeaderStyle");
				IStyle tableHeader = workbook.Styles.Add("TableHeaderStyle");

				pageHeader.Font.RGBColor = System.Drawing.Color.FromArgb(0, 83, 141, 213);
				pageHeader.Font.FontName = "Calibri";
				pageHeader.Font.Size = 18;
				pageHeader.Font.Bold = true;
				pageHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
				pageHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;

				tableHeader.Font.Color = ExcelKnownColors.White;
				tableHeader.Font.Bold = true;
				tableHeader.Font.Size = 11;
				tableHeader.Font.FontName = "Calibri";
				tableHeader.HorizontalAlignment = ExcelHAlign.HAlignCenter;
				tableHeader.VerticalAlignment = ExcelVAlign.VAlignCenter;
				tableHeader.Color = System.Drawing.Color.FromArgb(0, 118, 147, 60);
				tableHeader.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
				tableHeader.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
				tableHeader.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
				tableHeader.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;

				//Apply style to the header
				sheet["A1"].Text = "Employee records";
				sheet["A1"].CellStyle = pageHeader;
				sheet["A1"].CellStyle.Font.Bold = true;
				sheet["A1"].CellStyle.Font.Size = 16;


				sheet["A1:G1"].Merge();
				sheet["A2"].Text = "Id";
				sheet["B2"].Text = "Date";
				sheet["C2"].Text = "First name";
				sheet["D2"].Text = "Last name";
				sheet["E2"].Text = "User name";
				sheet["F2"].Text = "City";
				sheet["G2"].Text = "Country";
				sheet["A2:G2"].CellStyle = tableHeader;

				sheet.UsedRange.AutofitColumns();

				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.InitialDirectory = "c:\\";
				saveFileDialog.Filter = "Excel files(*.xlsx)|*.xlsx";
				saveFileDialog.DefaultExt = ".xlsx";
				if (saveFileDialog.ShowDialog() == false)
					MessageBox.Show("Экспорт отменён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				else {
					workbook.SaveAs(saveFileDialog.FileName);
				}
			}
		}

		// in this method showing data with filter
		private void Filter_Accept_Button_Click(object sender, RoutedEventArgs e) {
			ExportEmployees = SearchEmployees();
			EmployeesGrid.ItemsSource = ExportEmployees.ToBindingList();
		}

		// in this method we create condition that we are using in where request and return filtered data
		private ObservableCollection<Employee> SearchEmployees() {
			var predicate = PredicateBuilder.New<Employee>(e => true);

			if (DatePickerEmployee.Text != "") {
				predicate.And(e => e.Date == Convert.ToDateTime(DatePickerEmployee.SelectedDate, new CultureInfo("ru-Ru")));
			}
			if (EmployeeFirstName.Text != "") {
				predicate.And(e => e.FirstName == EmployeeFirstName.Text);
			}
			if (EmployeeLastName.Text != "") {
				predicate.And(e => e.LastName == EmployeeLastName.Text);
			}
			if (EmployeeUserName.Text != "") {
				predicate.And(e => e.UserName == EmployeeUserName.Text);
			}
			if (EmployeeCity.Text != "") {
				predicate.And(e => e.City == EmployeeCity.Text);
			}
			if (EmployeeCountry.Text != "") {
				predicate.And(e => e.Country == EmployeeCountry.Text);
			}

			return new ObservableCollection<Employee>(OriginalEmployees.Where<Employee>(predicate));

		}

		// showing all data
		private void Filter_Clear_Button_Click(object sender, RoutedEventArgs e) {
			DatePickerEmployee.Text = "";
			EmployeeFirstName.Text = "";
			EmployeeLastName.Text = "";
			EmployeeUserName.Text = "";
			EmployeeCity.Text = "";
			EmployeeCountry.Text = "";
			ExportEmployees = OriginalEmployees;
			EmployeesGrid.ItemsSource = OriginalEmployees.ToBindingList();

		}
	}
}
