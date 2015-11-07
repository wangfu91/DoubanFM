using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface IUserService
    {
        Task<User> GetUserInfo(string userId, string accessToken);
    }
}
