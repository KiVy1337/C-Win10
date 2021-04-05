using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Services {
	public interface IDataService<T> {
		Task<IEnumerable<T>> GetAll();
		Task<T> Get(int id);
		Task<T> Create(T entity);
		Task<int> Update(T entity);
		Task<bool> DeleteRange(IEnumerable<T> entities);
	}
}
