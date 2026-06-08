using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Drawing;

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
            public string? Avatar { get; set; } // Base64-encoded avatar image
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

        public static bool Register(string username, string password, out string message, string? avatarPath = null)
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

            // Convert image file to base64 if avatar path is provided
            string? avatarBase64 = null;
            if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
            {
                try
                {
                    var imageBytes = File.ReadAllBytes(avatarPath);
                    avatarBase64 = Convert.ToBase64String(imageBytes);
                }
                catch
                {
                    // If avatar fails to load, continue without it
                }
            }

            records.Add(new AccountRecord 
            { 
                Username = username, 
                Salt = salt, 
                PasswordHash = hash,
                Avatar = avatarBase64
            });
            SaveAll(records);
            message = "Registered successfully";
            return true;
        }

        public static bool Authenticate(string username, string password)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return false;

            var records = LoadAll();
            // If there are no accounts yet, create the first account automatically so the
            // client can log in on a fresh install without requiring a separate registration step.
            if (!records.Any())
            {
                // ignore the out message
                _ = Register(username, password, out _);
                return true;
            }

            var rec = records.FirstOrDefault(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase));
            if (rec == null) return false;
            var hash = Hash(password, rec.Salt);
            return hash == rec.PasswordHash;
        }

        /// <summary>Gets the avatar in base64 format for a user</summary>
        public static string? GetAvatar(string username)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username)) return null;

            var records = LoadAll();
            var rec = records.FirstOrDefault(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase));
            return rec?.Avatar;
        }

        /// <summary>Sets the avatar for a user from an image file path</summary>
        public static bool SetAvatar(string username, string imagePath, out string message)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username)) { message = "Username required"; return false; }

            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                message = "Invalid image file path";
                return false;
            }

            try
            {
                var records = LoadAll();
                var rec = records.FirstOrDefault(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase));
                if (rec == null) { message = "User not found"; return false; }

                var imageBytes = File.ReadAllBytes(imagePath);
                rec.Avatar = Convert.ToBase64String(imageBytes);
                SaveAll(records);
                message = "Avatar updated successfully";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Error setting avatar: {ex.Message}";
                return false;
            }
        }

        /// <summary>Converts base64 avatar data to Image object</summary>
        public static Image? ConvertBase64ToImage(string? base64String)
        {
            if (string.IsNullOrEmpty(base64String)) return null;

            try
            {
                var imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                return Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Removes the avatar for a user</summary>
        public static bool RemoveAvatar(string username, out string message)
        {
            username = username?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(username)) { message = "Username required"; return false; }

            try
            {
                var records = LoadAll();
                var rec = records.FirstOrDefault(r => string.Equals(r.Username, username, StringComparison.OrdinalIgnoreCase));
                if (rec == null) { message = "User not found"; return false; }

                rec.Avatar = null;
                SaveAll(records);
                message = "Avatar removed successfully";
                return true;
            }
            catch (Exception ex)
            {
                message = $"Error removing avatar: {ex.Message}";
                return false;
            }
        }
    }
}
