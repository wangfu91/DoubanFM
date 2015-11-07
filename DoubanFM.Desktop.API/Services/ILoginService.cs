using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface ILoginService
    {
        Task<LoginResult> LoginWithEmail(string email, string password);
    }
}
