using System.Collections.Generic;
using System.Linq;

namespace ChatCommon
{
    public class HistoryService
    {
        private readonly MessageRepository _repo;

        public HistoryService(MessageRepository repo)
        {
            _repo = repo;
        }

        public List<ChatMessage> LoadPublic(int count = 50)
        {
            return _repo.GetPublicMessages(count);
        }

        public List<ChatMessage> LoadPrivate(
            string user1,
            string user2,
            int count = 50)
        {
            var list = _repo.GetPrivateMessages(user1, 500);

            return list
                .Where(x =>
                    (x.Sender == user1 && x.Receiver == user2)
                    ||
                    (x.Sender == user2 && x.Receiver == user1))
                .TakeLast(count)
                .ToList();
        }
    }
}