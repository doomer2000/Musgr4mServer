using MServer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MServer.Models;
using MServer.Repositoies.Interfaces;
using System.Data.Entity;

namespace MServer.Repositoies
{
    public class UserService : IUserService
    {
        private Wheel_Context context;

        public UserService(Wheel_Context context)
        {
            this.context = context;
        }


        public async Task<Device> AddDevice(Device device)
        {
            device = context.Devices.Add(device);
            await context.SaveChangesAsync();
            return device;
        }

        public async void AddDeviceToUser(User user, Device device)
        {
            user.Devices.Add(device);
            await context.SaveChangesAsync();
        }

        public async Task<User> IsUserExist(string login, string password)
        {
            User user = await context.Users.FirstOrDefaultAsync<User>(d => d.Login == login && d.Password == password);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> TryLogin(string login, string password)
        {
            User user = await context.Users.Include("UserFriends").AsNoTracking().FirstOrDefaultAsync(d => d.Login == login && d.Password == password);
            if (user != null)
            {
                user.Password = string.Empty;
                user.MobileNum = string.Empty;
                user.UserMusic = null;
                user.Devices = null;
                //user.UserFriends = null;
                return user;
            }
            return null;
        }

        //public async Task<ICollection<Friend>> GetFriends(int id)
        //{
        //    var a = await context.Friends.Where(f => f.UserFriend.Id == id).FirstOrDefault();
        //    return a;
        //}


    }
}
