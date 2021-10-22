using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.SignalR
{
    public class PresenceTracker
    {
        /// <summary>
        /// Dictionnaire avec comme clé --> username utilisateur
        ///                         value --> clé de connection de la session
        /// </summary>
        private static readonly Dictionary<string, List<string>> OnLineUsers = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            lock (OnLineUsers)
            {
                if (OnLineUsers.ContainsKey(username))
                {
                    OnLineUsers[username].Add(connectionId);
                }
                else
                {
                    OnLineUsers.Add(username, new List<string> { connectionId });
                    isOnline = true;
                }
            }
            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (OnLineUsers)
            {
                if (!OnLineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

                OnLineUsers[username].Remove(connectionId);
                if (OnLineUsers[username].Count == 0)
                {
                    OnLineUsers.Remove(username);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        /// <summary>
        /// Récupération des éléments de connection de chaque user présent dans le dictionnaire
        /// </summary>
        /// <returns></returns>
        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnLineUsers)
            {
                onlineUsers = OnLineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock(OnLineUsers)
            {
                connectionIds = OnLineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }

    }
}
