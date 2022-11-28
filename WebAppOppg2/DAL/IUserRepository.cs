using System.Threading.Tasks;
using WebAppOppg2.Models;
using static WebAppOppg2.DAL.UserRepository;

namespace WebAppOppg2.DAL
{
    public interface IUserRepository
    {
        Task<bool> LoggInn(User user);
        Task<bool> Register(User user);
    }
}