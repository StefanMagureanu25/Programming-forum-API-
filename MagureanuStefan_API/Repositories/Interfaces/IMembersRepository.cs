using MagureanuStefan_API.Models;

namespace MagureanuStefan_API.Repositories.Interfaces
{
    public interface IMembersRepository
    {
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member> GetMemberByIdAsync(Guid id);
        Task CreateMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Guid id, Member member);
        Task<Member> UpdateMemberPartiallyAsync(Guid id, Member member); 
        Task<bool> DeleteMemberAsync(Guid id);
    }
}
