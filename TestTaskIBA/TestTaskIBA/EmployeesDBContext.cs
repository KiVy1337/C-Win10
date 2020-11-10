using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TestTaskIBA {
	public class EmployeesDBContext : DbContext {

		// constructor with different ways to initialize data
		public EmployeesDBContext(int mode) : base("Employees") {
			if (mode == 0) {
				Database.SetInitializer<EmployeesDBContext>(new DropCreateDatabaseAlways<EmployeesDBContext>());
			}
			else {
				Database.SetInitializer<EmployeesDBContext>(new CreateDatabaseIfNotExists<EmployeesDBContext>());
			}

		}

		public DbSet<Employee> Employees { get; set; }

	}
}
