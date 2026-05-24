using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChatClient
{
    public static class AccountManager
    {
        private static readonly string DataFile = Path.Combine(AppContext.BaseDirectory, "accounts.json");

        private class AccountRecord
        {
            public string Username { get; set; } = string.Empty;
            public string Salt { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
        }

        private static List<AccountRecord> LoadAll()
        {
            try
            {
                if (!File.Exists(DataFile)) return new List<AccountRecord>();
                var json = File.ReadAllText(DataFile);
                return JsonSerializer.Deserialize<List<AccountRecord>>(json) ?? new List<AccountRecord>();
            }
            catch
            {
                return new List<AccountRecord>();
            }
        }

        private static void SaveAll(List<AccountRecord> records)
        {
            var json = JsonSerializer.Serialize(records);
            File.WriteAllText(DataFile, json);
        }

        private static string CreateSalt(int size = 16)
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[size];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string Hash(string text, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text + salt);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Register(string username, string password, out string message)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username)) { message = "Username required"; return false; }
            if (string.IsNullOrEmpty(password)) { message = "Password required"; return false; }

            var records = LoadAll();
            if (records.Any(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase)))
            {
                message = "Username already exists";
                return false;
            }

            var salt = CreateSalt();
            var hash = Hash(password, salt);
            records.Add(new AccountRecord { Username = username, Salt = salt, PasswordHash = hash });
            SaveAll(records);
            message = "Registered successfully";
            return true;
        }

        public static bool Authenticate(string username, string password)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            var records = LoadAll();
            var rec = records.FirstOrDefault(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase));
            if (rec == null) return false;
            var hash = Hash(password, rec.Salt);
            return hash == rec.PasswordHash;
        }
    }
}
