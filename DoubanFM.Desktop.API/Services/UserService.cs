using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class UserService : ServiceBase, IUserService
    {
        public async Task<User> GetUserInfo(string userId, string token, long expire)
        {
            var param = new UserParams
            {
                user_id = userId,
                token = token,
                expire = expire
            };

            return await Get<User>(UserInfoRequestPath, param);

        }

    }

}
