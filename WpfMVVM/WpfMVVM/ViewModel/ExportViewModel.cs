using LinqKit;
using Microsoft.Win32;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WpfMVVM.Models;

namespace WpfMVVM.ViewModel {
	public class ExportViewModel : INotifyPropertyChanged {
        private ObservableCollection<Employee> originalList;
        private ObservableCollection<Employee> exportList;
        private Employee filter;
        private RelayCommand filteracceptionCommand;
        private RelayCommand filtercleaningCommand;
        private RelayCommand exportinxamlCommand;
        private RelayCommand exportinexcelCommand;

        public ObservableCollection<Employee> OriginalList {
            get { return originalList; }
            set {
                originalList = value;
                OnPropertyChanged("OriginalList");
            }
        }

        public ObservableCollection<Employee> ExportList {
            get { return exportList; }
            set {
                exportList = value;
                OnPropertyChanged("ExportList");
            }
        }
        public Employee Filter {
            get { return filter; }
            set {
                filter = value;
                OnPropertyChanged("Filter");
            }
        }

        public ExportViewModel() {
            Messenger.Default.Register<ObservableCollection<Employee>>(this, GetEmployeeList, "GetEmployeeList");
            Filter = new Employee(DateTime.Today.ToString(), "", "", "", "", "");
        }

        public RelayCommand FilterAcceptionCommand {
            get {
                return filteracceptionCommand ??
                  (filteracceptionCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      var predicate = PredicateBuilder.New<Employee>(e => true);

                      if (emp.Date != null) {
                          predicate.And(e => e.Date == emp.Date);
                      }
                      if (emp.FirstName != "") {
                          predicate.And(e => e.FirstName == emp.FirstName);
                      }
                      if (emp.LastName != "") {
                          predicate.And(e => e.LastName == emp.LastName);
                      }
                      if (emp.UserName != "") {
                          predicate.And(e => e.UserName == emp.UserName);
                      }
                      if (emp.City != "") {
                          predicate.And(e => e.City == emp.City);
                      }
                      if (emp.Country != "") {
                          predicate.And(e => e.Country == emp.Country);
                      }

                      ExportList = new ObservableCollection<Employee>(OriginalList.Where<Employee>(predicate));
                  }));
            }
        }

        public RelayCommand FilterCleaningCommand {
            get {
                return filtercleaningCommand ??
                  (filtercleaningCommand = new RelayCommand(obj => {
                      Employee emp = obj as Employee;
                      filter.Date = null;
                      filter.FirstName = "";
                      filter.LastName = "";
                      filter.UserName = "";
                      filter.City = "";
                      filter.Country = "";
                  }));
            }
        }

        public RelayCommand ExportInXAMLCommand {
            get {
                return exportinxamlCommand ??
                  (exportinxamlCommand = new RelayCommand( async obj => {
                      await Task.Run(() => XML_ExportAsync(ExportList)); // call async method
                  },
                  (obj) => {
                      if (ExportList.Count > 0)
                          return true;
                      else
                          return false;
                  }
                  ));
            }
        }

        public RelayCommand ExportInExcelCommand {
            get {
                return exportinexcelCommand ??
                  (exportinexcelCommand = new RelayCommand(async obj => {
                      await Task.Run(() => Excel_ExportAsync(ExportList)); // call async method
                  },
                  (obj) => {
                      if (ExportList.Count > 0)
                          return true;
                      else
                          return false;
                  }
                  ));
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

        // method that export data to xml
        private void XML_ExportAsync(ObservableCollection<Employee> export_employees) {
            XDocument xdoc = new XDocument();
            XElement xrecords = new XElement("Records"); // creating a root of xml document 

            // creating elements of xml document and filling int with data from exported collection
            foreach (Employee emp in export_employees) {
                XElement xrecord = new XElement("Record");
                XAttribute xid = new XAttribute("id", emp.Id);
                XElement xdate = new XElement("Date", emp.Date);
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

        private void GetEmployeeList(ObservableCollection<Employee> emps) {
            OriginalList = emps;
            ExportList = emps;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
