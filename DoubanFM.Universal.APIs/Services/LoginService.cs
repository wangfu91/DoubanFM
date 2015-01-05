using DoubanFM.Universal.APIs.Models;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public class LoginService : ServiceBase, ILoginService
    {
        public async Task<LoginResult> LoginWithEmail(string email, string password)
        {
            var param = new LoginParams
            {
                email = email,
                password = password
            };

            return await Post<LoginResult>(LoginReqPath, param);
        }


        public async Task<LoginResult> LoginWithUserName(string userName, string password)
        {
            var param = new LoginParams
            {
                username = userName,
                password = password
            };

            return await Post<LoginResult>(LoginReqPath, param);
        }


    }
}
