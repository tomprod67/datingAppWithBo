using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikesRepository LikesRepository { get; }
        IAccountRepository AccountRepository { get; }
        Task<bool> Complete();
        bool HasChanges();

    }
}
