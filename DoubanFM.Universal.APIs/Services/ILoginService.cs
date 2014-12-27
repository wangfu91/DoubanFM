using DoubanFM.Universal.APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public interface ILoginService
    {
        Task<LoginResult> LoginWithEmail(string email, string password);

        Task<LoginResult> LoginWithUserName(string userName, string password);
    }
}
