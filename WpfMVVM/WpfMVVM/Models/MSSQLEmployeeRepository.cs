using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Models {
	class MSSQLEmployeeRepository : IRepository{
		private EmployeeDBContext db;
		private bool disposed = false;

		public MSSQLEmployeeRepository(int mode) {
			db = new EmployeeDBContext(mode);
		}

		public ObservableCollection<Employee> GetEmployeeList() {
			return db.Employees.Local;
		}

		public void CreateRange(IEnumerable<Employee> employees) {
			db.Employees.AddRange(employees);
		}
		public void Create(Employee emp) {
			db.Employees.Add(emp);
		}

		public void Update(Employee emp) {
			db.Entry(emp).State = EntityState.Modified;
		}

		public void DeleteRange(IEnumerable<Employee> employees) {
			db.Employees.RemoveRange(employees);
		}

		public void Save() {
			db.SaveChanges();
		}
		public bool IsEmployeeInDB(Employee emp) {
			if (db.Employees.Any(e => e.Id == emp.Id)) {
				return true;
			}
			else {
				return false;
			}
		}

		public virtual void Dispose(bool disposing) {
			if (!this.disposed) {
				if (disposing) {
					db.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

	}
}
