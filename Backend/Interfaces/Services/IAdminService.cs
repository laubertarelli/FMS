using Backend.Dtos.User;
using System.Security.Claims;

namespace Backend.Interfaces.Services
{
    public interface IAdminService
    {
        Task<UserDto?> Update(UpdateAdminDto updateDto);
        Task<bool> Delete(string email);
        Task<List<UserDto>> GetAll(int page);
        Task<UserDto?> GetById(string id);
        Task<int> CountAll();
        Task<IList<Claim>?> ManageUserClaims(AdminClaimsDto claimsDto);
    }
}
