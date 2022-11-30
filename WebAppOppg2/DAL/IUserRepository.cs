using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public interface IUserRepository
    {//disse trengs for å kunne referer til controller, repository
        Task<string> LoggInn(User user);
        Task<bool> Register(User user);
    }
}