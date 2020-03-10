using SdvCode.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SdvCode.Areas.Administration.Services
{
    public class UsersPenaltiesService : IUsersPenaltiesService
    {
        private readonly ApplicationDbContext db;

        public UsersPenaltiesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> BlockUser(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);

            if (user != null && user.IsBlocked == false)
            {
                user.IsBlocked = true;
                this.db.Users.Update(user);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UnblockUser(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.UserName == username);

            if (user != null && user.IsBlocked == true)
            {
                user.IsBlocked = false;
                this.db.Users.Update(user);
                await this.db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public ICollection<string> GetAllBlockedUsers()
        {
            return this.db.Users.Where(u => u.IsBlocked == true).Select(x => x.UserName).ToList();
        }

        public ICollection<string> GetAllNotBlockedUsers()
        {
            return this.db.Users.Where(u => u.IsBlocked == false).Select(x => x.UserName).ToList();
        }
    }
}