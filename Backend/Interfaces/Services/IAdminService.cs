using Backend.Dtos.Admin;
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
        List<PermissionDto> GetAllClaims();
        List<PermissionDto> GetAllRoles();
        Task<IList<Claim>?> GetUserClaims(string id);
        Task<IList<string>?> GetUserRoles(string id);
        Task<IList<Claim>?> ManageUserClaims(AdminClaimsDto claimsDto);
        Task<IList<string>?> ManageUserRoles(AdminClaimsDto userDto);
    }
}
