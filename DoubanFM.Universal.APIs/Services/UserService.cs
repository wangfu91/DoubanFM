using DoubanFM.Universal.APIs.Models;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public class UserService : ServiceBase,IUserService
    {
        public async Task<User> GetUserInfo(string userId, string token, string expire)
        {
            var param = new UserParams
            {
                user_id = userId,
                token = token,
                expire = expire
            };

            return await Get<User>(UserReqPath, param);
        }

    }

}
