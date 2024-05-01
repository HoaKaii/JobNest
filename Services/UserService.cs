using JobsFinder_Main.Identity;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Model.Services;

namespace JobsFinder_Main.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GetUserNameByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.UserName;
        }
    }
}
