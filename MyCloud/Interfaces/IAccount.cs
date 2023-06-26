using MyCloud.ViewModels.Account;
using System.Security.Claims;
using System.Threading.Tasks;
using MyCloud.Response;

namespace MyCloud.Interfaces
{
    public interface IAccount
    {
        Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

        Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

        Task<BaseResponse<IEnumerable<UserViewModel>>> GetUsers();

        Task<IBaseResponse<bool>> DeleteUser(long id);
    }
}
