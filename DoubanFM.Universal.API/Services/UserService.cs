using DoubanFM.Universal.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.API.Services
{
    public class UserService : ServiceBase
    {
        private const string LoginPath = "j/app/login";
        private const string UserInfoPath = "/j/app/radio/user_info";
        public async Task<LoginResult> LoginWithEmail(string email, string password)
        {
            var param = new LoginParams
            {
                email = email,
                password = password
            };

            return await Post<LoginResult>(LoginPath, param);
        }


        public async Task<LoginResult> LoginWithUserName(string userName, string password)
        {
            var param = new LoginParams
            {
                username = userName,
                password = password
            };

            return await Post<LoginResult>(LoginPath, param);
        }

        public async Task<User> GetUserInfo(string userId, string token, string expire)
        {
            var param = new UserParams
            {
                user_id = userId,
                token = token,
                expire = expire
            };

            return await Get<User>(UserInfoPath, param);
        }

    }

}
