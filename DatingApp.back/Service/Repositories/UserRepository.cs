using System.Linq;
using System.Threading.Tasks;
using DB.Entities;
using DB.Data;
using Service.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Service.Helpers;
using System;
using System.Collections.Generic;
using Service.BOs;

namespace Service.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        /// <summary>
        /// Récupération d'un membre DTO pour la requête de GetUSerByUsername()
        /// </summary>
        public async Task<MemberBo> GetMemberAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberBo>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Filtre de la page principale -> visualisation des personnes en fonction des paramètre choisi dans le filtre
        /// </summary>
        public async Task<PagedList<MemberBo>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUsername);     //  Ne pas afficher la personne connecté dans la liste des personnes
            query = query.Where(u => u.Gender == userParams.Gender);                //  Affiche le genre opposé de la personne

            var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),       // Pour l'élément lastCreated
                _ => query.OrderByDescending(u => u.LastActive)             // Pour l'élément par défaut (lastActive)
            };

            //  Classe les personnes par page et permet de return la bonne liste de personne en fonction de la page que la personne demande dans le client
            return await PagedList<MemberBo>.CreateAsync(query.ProjectTo<MemberBo>(mapper.ConfigurationProvider).AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserGender(string username)
        {
            return await context.Users.Where(x => x.UserName == username).Select(x => x.Gender).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        
    }
}