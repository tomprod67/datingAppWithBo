using AutoMapper;
using AutoMapper.QueryableExtensions;
using DB.Data;
using DB.Entities;
using DTO.DTOs;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using Service.Helpers;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Recupération des messages par ordre d'envoi
        /// </summary>
        /// <param name="messageParams">Object contenant le username du message et l'état du message (ex : unread etc....)</param>
        /// <returns></returns>
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            IQueryable<MessageDto> query = context.Messages
                .OrderByDescending(m => m.MessageSent)
                .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };

            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }


        /// <summary>
        /// Récupération de la conversation privé entre 2 personnes 
        /// </summary>
        /// <param name="currentUsername"></param>
        /// <param name="recipientUsername"></param>
        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            List<MessageDto> messages = await context.Messages
                .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
                    && m.Sender.UserName == recipientUsername 
                    || m.Recipient.UserName == recipientUsername 
                    && m.Sender.UserName == currentUsername && m.SenderDeleted == false
                )
                .OrderBy(m => m.MessageSent)
                .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            List<MessageDto> unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (MessageDto message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return messages;
        }

        /// <summary>
        /// Création du groupe de discussion privé
        /// </summary>
        /// <param name="group">username destination - username expediteur</param>

        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        /// <summary>
        /// Récupération de l'id de connection du group
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public async Task<Connection> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        /// <summary>
        /// Récupération des messages priivées entre 2 utilisateurs
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>Id et le nom du groupe de discussion</returns>
        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        /// <summary>
        /// Suppression de l'id de connection du group de message
        /// </summary>
        /// <param name="connection"></param>
        public void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }


        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }
    }
}
