using Microsoft.Data.Sqlite;

namespace ChatCommon
{
    /// <summary>
    /// Quản lý lưu trữ tin nhắn vào SQLite.
    /// Đặt trong ChatCommon để cả Server (ghi) và Client (đọc Phase 2) đều dùng được.
    /// </summary>
    public class MessageRepository : IDisposable
    {
        private readonly SqliteConnection _conn;

        /// <summary>
        /// Khởi tạo repository, tạo/mở file SQLite và đảm bảo bảng tồn tại.
        /// </summary>
        /// <param name="dbPath">Đường dẫn file .db (mặc định nằm cạnh exe).</param>
        public MessageRepository(string dbPath = "chat_messages.db")
        {
            string fullPath = Path.IsPathRooted(dbPath)
                ? dbPath
                : Path.Combine(AppContext.BaseDirectory, dbPath);

            _conn = new SqliteConnection($"Data Source={fullPath}");
            _conn.Open();
            EnsureTable();
        }

        /// <summary>
        /// Tạo bảng ChatMessages nếu chưa tồn tại.
        /// </summary>
        private void EnsureTable()
        {
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ChatMessages (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                    Sender      TEXT    NOT NULL,
                    Receiver    TEXT,
                    Content     TEXT    NOT NULL,
                    MessageType TEXT    DEFAULT 'public',
                    SentAt      DATETIME DEFAULT CURRENT_TIMESTAMP
                );

                -- Index để tăng tốc truy vấn khi load lịch sử (Phase 2)
                CREATE INDEX IF NOT EXISTS idx_chatmsg_sender   ON ChatMessages(Sender);
                CREATE INDEX IF NOT EXISTS idx_chatmsg_receiver ON ChatMessages(Receiver);
                CREATE INDEX IF NOT EXISTS idx_chatmsg_sentat   ON ChatMessages(SentAt);
            ";
            cmd.ExecuteNonQuery();
        }

        // ═══════════════════════════════════════════════════════════════
        //  WRITE — Phase 1: Lưu tin nhắn
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Lưu 1 tin nhắn vào database.
        /// </summary>
        /// <param name="sender">Username người gửi.</param>
        /// <param name="receiver">Username người nhận (null = tin chung).</param>
        /// <param name="content">Nội dung tin nhắn.</param>
        /// <param name="messageType">"public", "private", "server", "system".</param>
        public void SaveMessage(string sender, string? receiver, string content, string messageType)
        {
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ChatMessages (Sender, Receiver, Content, MessageType, SentAt)
                VALUES (@sender, @receiver, @content, @type, @sentAt)
            ";
            cmd.Parameters.AddWithValue("@sender", sender);
            cmd.Parameters.AddWithValue("@receiver", (object?)receiver ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@content", content);
            cmd.Parameters.AddWithValue("@type", messageType);
            cmd.Parameters.AddWithValue("@sentAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }

        // ═══════════════════════════════════════════════════════════════
        //  CLEANUP — Tự động xoá tin nhắn cũ
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Xoá tin nhắn cũ hơn khoảng thời gian chỉ định.
        /// </summary>
        /// <returns>Số dòng đã xoá.</returns>
        public int PurgeOlderThan(TimeSpan retention)
        {
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = "DELETE FROM ChatMessages WHERE SentAt < @cutoff";
            cmd.Parameters.AddWithValue("@cutoff", (DateTime.Now - retention).ToString("yyyy-MM-dd HH:mm:ss"));
            return cmd.ExecuteNonQuery();
        }

        // ═══════════════════════════════════════════════════════════════
        //  READ — Phase 2: Load lịch sử (đã viết sẵn, chưa gọi)
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// Lấy tin nhắn chung (public + server + system) gần nhất.
        /// Phase 2: Server gọi method này sau khi Client login, rồi gửi qua protocol HISTORY:.
        /// </summary>
        /// <param name="count">Số tin nhắn tối đa.</param>
        public List<ChatMessage> GetPublicMessages(int count = 50)
        {
        using var cmd = _conn.CreateCommand();

        cmd.CommandText = @"
        SELECT Id, Sender, Receiver, Content, MessageType, SentAt
        FROM ChatMessages
        WHERE MessageType = 'public'
        ORDER BY SentAt DESC
        LIMIT @count
        ";

            cmd.Parameters.AddWithValue("@count", count);

            return ReadMessages(cmd);
        }

        /// <summary>
        /// Lấy tin nhắn riêng liên quan đến 1 user (cả gửi lẫn nhận).
        /// Phase 2: Server gọi method này để gửi lịch sử riêng cho đúng Client.
        /// </summary>
        /// <param name="username">Username cần lấy.</param>
        /// <param name="count">Số tin nhắn tối đa.</param>
        public List<ChatMessage> GetPrivateMessages(int count = 100)
        {
            using var cmd = _conn.CreateCommand();

            cmd.CommandText = @"
            SELECT Id, Sender, Receiver, Content, MessageType, SentAt
            FROM ChatMessages
            WHERE MessageType='private'
            ORDER BY SentAt DESC
            LIMIT @count";

            cmd.Parameters.AddWithValue("@count", count);

            return ReadMessages(cmd);
        }

        public List<ChatMessage> GetPrivateMessages(
        string username,
        int count = 50)
        {
            using var cmd = _conn.CreateCommand();

            cmd.CommandText = @"
            SELECT Id, Sender, Receiver, Content, MessageType, SentAt
            FROM ChatMessages
            WHERE MessageType='private'
            AND (Sender=@user OR Receiver=@user)
            ORDER BY SentAt DESC
            LIMIT @count";

            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@count", count);

            return ReadMessages(cmd);
        }


        public List<ChatMessage> GetPrivateMessagesBetween(string user1,string user2,int count = 100)
        {
            using var cmd = _conn.CreateCommand();

            cmd.CommandText = @"
            SELECT Id, Sender, Receiver, Content, MessageType, SentAt
            FROM ChatMessages
            WHERE MessageType='private'
            AND
            (
                (Sender=@u1 AND Receiver=@u2)
                OR
                (Sender=@u2 AND Receiver=@u1)
            )
            ORDER BY SentAt DESC
            LIMIT @count";

            cmd.Parameters.AddWithValue("@u1", user1);
            cmd.Parameters.AddWithValue("@u2", user2);
            cmd.Parameters.AddWithValue("@count", count);

            return ReadMessages(cmd);
        }

        /// <summary>
        /// Lấy toàn bộ lịch sử liên quan đến 1 user (chung + riêng), sắp xếp theo thời gian.
        /// Phase 2: Dùng method này nếu muốn gửi tất cả 1 lần cho Client.
        /// </summary>
        public List<ChatMessage> GetAllMessagesForUser(string username, int count = 100)
        {
            using var cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, Sender, Receiver, Content, MessageType, SentAt
                FROM ChatMessages
                WHERE Receiver IS NULL
                   OR (MessageType = 'private' AND (Sender = @user OR Receiver = @user))
                ORDER BY SentAt DESC
                LIMIT @count
            ";
            cmd.Parameters.AddWithValue("@user", username);
            return ReadMessages(cmd);
        }

        // ═══════════════════════════════════════════════════════════════
        //  HELPER
        // ═══════════════════════════════════════════════════════════════

        private static List<ChatMessage> ReadMessages(SqliteCommand cmd)
        {
            var list = new List<ChatMessage>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ChatMessage
                {
                    Id = reader.GetInt64(0),
                    Sender = reader.GetString(1),
                    Receiver = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Content = reader.GetString(3),
                    MessageType = reader.GetString(4),
                    SentAt = DateTime.TryParse(reader.GetString(5), out var dt) ? dt : DateTime.Now
                });
            }
            // Đảo lại cho đúng thứ tự thời gian (cũ → mới) vì query ORDER BY DESC
            list.Reverse();
            return list;
        }

        // ═══════════════════════════════════════════════════════════════
        //  DISPOSE
        // ═══════════════════════════════════════════════════════════════

        public void Dispose()
        {
            try { _conn.Close(); } catch { }
            try { _conn.Dispose(); } catch { }
        }
    }
}
