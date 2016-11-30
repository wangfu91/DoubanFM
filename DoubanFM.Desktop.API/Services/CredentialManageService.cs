using DoubanFM.Desktop.API.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class CredentialManageService : ICredentialManageService
    {
        private static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private static readonly string DoubanFmFolder = Path.Combine(AppDataFolder, "DoubanFM");

        private static readonly string DataFile = Path.Combine(DoubanFmFolder, "data.json");

        public async Task<LoginResult> LoadCredentialAsync()
        {
            if (!File.Exists(DataFile)) return null;

            using (var reader = new StreamReader(DataFile))
            {
                var json = await reader.ReadToEndAsync();
                return string.IsNullOrEmpty(json) 
                    ? null 
                    : JsonConvert.DeserializeObject<LoginResult>(json);
            }
        }


        public async Task SaveCredentialAsync(LoginResult result)
        {
            if (!Directory.Exists(DoubanFmFolder))
                Directory.CreateDirectory(DoubanFmFolder);
            var dataFile = Path.Combine(DoubanFmFolder, "data.json");
            using (var writer = new StreamWriter(new FileStream(dataFile, FileMode.Create, FileAccess.Write, FileShare.None)))
            {
                await writer.WriteLineAsync(JsonConvert.SerializeObject(result));
            }
        }

        public void DeleteSavedCredential()
        {
            if (!File.Exists(DataFile)) return;
            using (new FileStream(DataFile, FileMode.Open, FileAccess.Read, FileShare.Delete))
            {
                File.Delete(DataFile);
            }
        }
    }
}
