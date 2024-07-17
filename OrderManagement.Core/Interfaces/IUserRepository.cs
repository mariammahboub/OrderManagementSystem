using OrderManagement.Core.Entities;
using System.Threading.Tasks;

namespace OrderManagement.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);

    }
}
