namespace library.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using library.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<IEnumerable<User>> GetActiveUsersAsync();
    Task<IEnumerable<User>> GetInactiveUsersAsync();
    Task<int> GetUserCountAsync();
    Task<int> GetAcceptedUserCountAsync();
    Task<User> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> ActivateUserAsync(int userId);
    Task<bool> DeactivateUserAsync(int userId);
    Task<bool> DeleteUserAsync(int userId);
}
