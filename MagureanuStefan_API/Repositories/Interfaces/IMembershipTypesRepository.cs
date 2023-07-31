using MagureanuStefan_API.Models;

namespace MagureanuStefan_API.Repositories.Interfaces
{
    public interface IMembershipTypesRepository
    {
        Task<IEnumerable<MembershipType>> GetAllMembershipTypesAsync();
        Task<MembershipType> GetMembershipTypeByIdAsync(Guid id);
        Task CreateMembershipTypeAsync(MembershipType membershipType);
        Task<bool> DeleteMembershipTypeAsync(Guid id);
        Task<MembershipType> UpdateMembershipTypeAsync(Guid id, MembershipType membershipType);
        Task<MembershipType> UpdateMembershipTypePartiallyAsync(Guid id, MembershipType membershipType);
    }
}
