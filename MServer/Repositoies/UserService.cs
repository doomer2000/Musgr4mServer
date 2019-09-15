using MServer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MServer.Models;
using MServer.Repositoies.Interfaces;
using System.Data.Entity;
using System.Data.SqlClient;

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
            User user = await context.Users.AsNoTracking().FirstOrDefaultAsync(d => d.Login == login && d.Password == password);
            if (user != null)
            {
                user.Password = string.Empty;
                user.MobileNum = string.Empty;
                user.UserFriends = await GetFriends(user.Id);
                return user;
            }
            return null;
        }

        public async Task<ICollection<Friend>> GetFriends(int id)
        {
            SqlParameter param = new SqlParameter("@Id", id);
            ICollection<Friend> friends = await context.Database.SqlQuery<Friend>("SELECT * FROM Friends WHERE Friends.User_Id = @Id", param).ToListAsync();
            foreach (Friend F in friends)
            {
                await context.Users.FindAsync(F._Friend_Id).ContinueWith(
                    t =>
                    {
                        User res = t.Result;
                        res.Password = string.Empty;
                        res.MobileNum = string.Empty;
                        F._Friend = res;
                    });
            }
            return friends;
        }


    }
}
