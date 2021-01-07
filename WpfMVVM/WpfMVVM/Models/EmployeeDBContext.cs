using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Models {
	public class EmployeeDBContext : DbContext {

		// constructor with different ways to initialize data
		public EmployeeDBContext(int mode) : base("Employees") {
			if (mode == 0) {
				Database.SetInitializer<EmployeeDBContext>(new DropCreateDatabaseAlways<EmployeeDBContext>());
			}
			else {
				Database.SetInitializer<EmployeeDBContext>(new CreateDatabaseIfNotExists<EmployeeDBContext>());
			}

		}
		public DbSet<Employee> Employees { get; set; }

	}
}
