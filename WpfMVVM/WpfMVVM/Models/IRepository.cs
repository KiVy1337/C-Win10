using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Models {
    interface IRepository : IDisposable {
        ObservableCollection<Employee> GetEmployeeList();
        void CreateRange(IEnumerable<Employee> items);
        bool IsEmployeeInDB(Employee emp);
        void Create(Employee item);
        void Update(Employee item);
        void DeleteRange(IEnumerable<Employee> items);
        void Save();
    }
}
