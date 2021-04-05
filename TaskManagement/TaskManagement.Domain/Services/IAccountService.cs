using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Models;

namespace TaskManagement.Domain.Services {
	public interface IAccountService :IDataService<Account> {
		Task<Account> GetByUsername(string username);
		Task<Account> GetByEmail(string email);

	}
}
