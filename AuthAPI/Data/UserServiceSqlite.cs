using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using MoneyTrackDatabaseAPI.DataAccess;
using MoneyTrackDatabaseAPI.Models;
using MoneyTrackDatabaseAPI.Services;

namespace MoneyTrackDatabaseAPI.Data
{
    public class UserServiceSqlite : IUserService
    {
        private UserCredentialsDbContext dbContext;
        private IAuthService _authService;
        private SecurityService _securityService;

        public UserServiceSqlite(UserCredentialsDbContext dbContext, IAuthService authService, SecurityService securityService)
        {
            this.dbContext = dbContext;
            _authService = authService;
            _securityService = securityService;
        }

        public async Task<User> Register(User user)
        {

            user = _securityService.HashCredentials(user);
            user.EmailConfirmed = false;
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            
            user.Salt = null;
            user.Password = null;
            user.HashVersion = null;
            return user;
        }

        public async Task<User> Delete()
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            User toRemove = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f => f.Id == _authService.AuthModel.UserId);
            dbContext.Remove(toRemove);
            await dbContext.SaveChangesAsync();
            
            return toRemove;
        }

        public async Task<User> Validate(string email, string password)
        {
            User found = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f => f.Email.Equals(email));
            if (!_securityService.VerifyHash(password, found.Salt, found.Password))
                throw new Exception("Password or email incorrect");
            found.Salt = null;
            found.Password = null;
            found.HashVersion = null;
            return found;
        }
        
        public async Task<User> Update(User user)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            if(user.Id!=_authService.AuthModel.UserId)
            {
                throw new Exception("Unauthorized");
            }
            User toUpdate = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f => f.Id == user.Id);
            toUpdate.Email = user.Email;
            toUpdate.Name = user.Name;
            toUpdate.Password = user.Password;
            dbContext.Update(toUpdate);
            await dbContext.SaveChangesAsync();
            toUpdate.Salt = null;
            toUpdate.Password = null;
            toUpdate.HashVersion = null;
            return toUpdate;
        }

        public async Task<User> AddDevice(string eui)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            User toUpdate = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f =>f.Id == _authService.AuthModel.UserId);
            var contains = toUpdate.Devices.Where(u => u.Eui.Equals(eui)).ToList();
            if(contains.Count==0)
            {
                toUpdate.Devices.Add(new Device(eui));
                await dbContext.SaveChangesAsync();
            }
            toUpdate.Salt = null;
            toUpdate.Password = null;
            toUpdate.HashVersion = null;
            return toUpdate;
        }

        public async Task<User> DeleteDevice(string eui)
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            User found = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f =>f.Id == _authService.AuthModel.UserId);
            Device deleted = found.Devices.Find(d=>d.Eui.Equals(eui));
            dbContext.Remove(deleted);
            await dbContext.SaveChangesAsync();
            found.Salt = null;
            found.Password = null;
            found.HashVersion = null;
            return found;
        }

        public async Task<List<Device>> GetDevices()
        {
            if(_authService.AuthModel==null)
            {
                throw new Exception("Access denied!");
            }
            User found = await dbContext.Users.Include(f=>f.Devices).FirstAsync(f =>f.Id == _authService.AuthModel.UserId);
            return found.Devices;
        }
    }
}