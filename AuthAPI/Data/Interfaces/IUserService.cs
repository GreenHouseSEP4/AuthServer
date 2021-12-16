using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTrackDatabaseAPI.Models;

namespace MoneyTrackDatabaseAPI.Data
{
    public interface IUserService
    {
        Task<User> Register(User user);
        Task<User> Delete();
        Task<User> Validate(string email,string password);
        Task<User> Update(User user);

        Task<User> AddDevice(string eui);
        Task<User> DeleteDevice(string eui);
        Task<List<Device>> GetDevices();
    }
}