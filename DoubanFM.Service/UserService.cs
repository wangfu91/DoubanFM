using DoubanFM.Data;
using DoubanFM.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Service
{
    public class UserService : BaseService
    {
        public async Task<LoginResult> LoginWithEmail(string email, string password)
        {
            var param = new LoginSvcParams
            {
                email = email,
                password = password
            };

            return await Post<LoginResult>(LoginRequstPath, param);
        }


        public async Task<LoginResult> LoginWithUserName(string userName, string password)
        {
            var param = new LoginSvcParams
            {
                username = userName,
                password = password
            };

           return await Post<LoginResult>(LoginRequstPath, param);
        }

        public async Task<User> GetUserInfo(string userId, string token, string expire)
        {
            var param = new UserSvcParams
            {
                user_id = userId,
                token = token,
                expire = expire
            };

            return await Get<User>(UserInfoRequestPath, param);
        }

    }
}
