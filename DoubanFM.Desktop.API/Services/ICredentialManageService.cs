using System.Threading.Tasks;
using DoubanFM.Desktop.API.Models;

namespace DoubanFM.Desktop.API.Services
{
    public interface ICredentialManageService
    {
        Task<LoginResult> LoadCredentialAsync();
        Task SaveCredentialAsync(LoginResult result);
        void DeleteSavedCredential();
    }
}