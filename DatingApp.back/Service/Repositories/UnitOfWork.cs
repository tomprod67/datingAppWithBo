using AutoMapper;
using DB.Data;
using Service.Interfaces;
using Service.Repository;
using System.Threading.Tasks;

namespace Service.Repositories
{
    public class UnitOfWork : IUnitOfWork
        /* 
         Cette classe permet d'unifier la création de context et des mappers 
         Elle permet de les créers une fois puis de les distribuers aux autres classes qui en découlent
         */
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(context, mapper);

        public IMessageRepository MessageRepository => new MessageRepository(context, mapper);

        public ILikesRepository LikesRepository => new LikesRepository(context);

        public IAccountRepository AccountRepository => new AccountRepository();

        /// <summary>
        /// Permet de faire la synchronisation des données quand on a fini 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Méthode si il y a eu des modifications à prendre en compte
        /// </summary>
        /// <returns></returns>
        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}
