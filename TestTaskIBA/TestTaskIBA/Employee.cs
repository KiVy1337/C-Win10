using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskIBA {
	public class Employee {
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string UserName { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public Employee(string[] fields) {
			Date = Convert.ToDateTime(fields[0], new CultureInfo("ru-Ru"));
			FirstName = fields[1];
			LastName = fields[2];
			UserName = fields[3];
			City = fields[4];
			Country = fields[5];
		}

		public Employee(string date, string firstname, string lastname, string surname, string city, string country) {
			Date = Convert.ToDateTime(date, new CultureInfo("ru-Ru"));
			FirstName = firstname;
			LastName = lastname;
			UserName = surname;
			City = city;
			Country = country;
		}



	}
}
