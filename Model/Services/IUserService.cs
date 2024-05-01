using System.Threading.Tasks;

namespace Model.Services
{
    public interface IUserService
    {
        Task<string> GetUserNameByUserId(string userId);
    }
}
