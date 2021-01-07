
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Models {
	public class Employee : INotifyPropertyChanged {
		private int id;
		private DateTime? date;
		private string firstName;
		private string lastName;
		private string userName;
		private string city;
		private string country;
		public int Id {
			get { return id; }
			set {
				id = value;
				OnPropertyChanged("Id");
			}
		}

		public DateTime? Date {
			get { return date; }
			set {
				date = value;
				OnPropertyChanged("Date");
			}
		}
		public string FirstName {
			get { return firstName; }
			set {
				firstName = value;
				OnPropertyChanged("FirstName");
			}
		}
		public string LastName {
			get { return lastName; }
			set {
				lastName = value;
				OnPropertyChanged("LastName");
			}
		}

		public string City {
			get { return city; }
			set {
				city = value;
				OnPropertyChanged("City");
			}
		}

		public string UserName {
			get { return userName; }
			set {
				userName = value;
				OnPropertyChanged("UserName");
			}
		}
		public string Country {
			get { return country; }
			set {
				country = value;
				OnPropertyChanged("Country");
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}


		public Employee(Employee emp) {
			Date = emp.Date;
			FirstName = emp.FirstName;
			LastName = emp.LastName;
			UserName = emp.UserName;
			City = emp.City;
			Country = emp.Country;
		}
		public Employee(string[] fields) {
			if (fields[0] == "") {
				Date = null;
			}
			else {
				Date = Convert.ToDateTime(fields[0], new CultureInfo("ru-Ru"));
			}
			FirstName = fields[1];
			LastName = fields[2];
			UserName = fields[3];
			City = fields[4];
			Country = fields[5];
		}

		public Employee(string date, string firstname, string lastname, string surname, string city, string country) {
			if (date == "") {
				Date = null;
			}
			else {
				Date = Convert.ToDateTime(date, new CultureInfo("ru-Ru"));
			}
			FirstName = firstname;
			LastName = lastname;
			UserName = surname;
			City = city;
			Country = country;
		}



	}
}
