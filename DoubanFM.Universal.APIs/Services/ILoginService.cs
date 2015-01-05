using DoubanFM.Universal.APIs.Models;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public interface ILoginService
    {
        Task<LoginResult> LoginWithEmail(string email, string password);

        Task<LoginResult> LoginWithUserName(string userName, string password);
    }
}
