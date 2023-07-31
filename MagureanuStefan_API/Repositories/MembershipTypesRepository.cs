using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Exceptions;
using MagureanuStefan_API.Helpers.Enums;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagureanuStefan_API.Repositories
{
    public class MembershipTypesRepository : IMembershipTypesRepository
    {
        private readonly ClubLibraDataContext _context;
        public MembershipTypesRepository(ClubLibraDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MembershipType>> GetAllMembershipTypesAsync()
        {
            return await _context.MembershipTypes.ToListAsync();
        }
        public async Task<MembershipType> GetMembershipTypeByIdAsync(Guid id)
        {
            return await _context.MembershipTypes.SingleOrDefaultAsync(x => x.IdMembershipType == id);
        }
        public async Task CreateMembershipTypeAsync(MembershipType membershipType)
        {
            if (membershipType.Name == null || membershipType.Description == null ||
                membershipType.SubscriptionLengthInMonths == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.MembershipType.WrongFormatPut);
            }
            if (await MembershipTypeWithGivenNameExists(membershipType.Name))
            {
                throw new ModelValidationException(ErrorMessagesEnum.MembershipType.MembershipTypeExists);
            }
            membershipType.IdMembershipType = Guid.NewGuid();
            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();
        }
        public async Task<MembershipType> UpdateMembershipTypeAsync(Guid id, MembershipType membershipType)
        {
            if (membershipType.Name == null || membershipType.Description == null ||
                membershipType.SubscriptionLengthInMonths == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.MembershipType.WrongFormatPut);
            }
            if (!await MembershipTypeWithGivenIdExists(id))
            {
                return null;
            }
            _context.MembershipTypes.Update(membershipType);
            await _context.SaveChangesAsync();
            return membershipType;
        }
        public async Task<MembershipType> UpdateMembershipTypePartiallyAsync(Guid id, MembershipType membershipType)
        {
            if (!await MembershipTypeWithGivenIdExists(id))
            {
                return null;
            }
            var membershipTypeFromDatabase = await GetMembershipTypeByIdAsync(id);
            bool isChanged = false;
            if (!String.IsNullOrEmpty(membershipType.Name) && membershipType.Name != membershipTypeFromDatabase.Name)
            {
                isChanged = true;
                membershipTypeFromDatabase.Name = membershipType.Name;
            }
            if (!String.IsNullOrEmpty(membershipType.Description) && membershipType.Description != membershipTypeFromDatabase.Description)
            {
                isChanged = true;
                membershipTypeFromDatabase.Description = membershipType.Description;
            }
            if (membershipType.SubscriptionLengthInMonths != null
                && membershipType.SubscriptionLengthInMonths != membershipTypeFromDatabase.SubscriptionLengthInMonths)
            {
                isChanged = true;
                membershipTypeFromDatabase.SubscriptionLengthInMonths = membershipType.SubscriptionLengthInMonths;
            }
            if (isChanged)
            {
                _context.MembershipTypes.Update(membershipType);
                await _context.SaveChangesAsync();
                return membershipTypeFromDatabase;
            }
            else
            {
                throw new ModelValidationException(ErrorMessagesEnum.MembershipType.ZeroUpdateToSave);
            }
        }
        public async Task<bool> DeleteMembershipTypeAsync(Guid id)
        {
            if (!await MembershipTypeWithGivenIdExists(id))
            {
                return false;
            }
            var membershipType = await GetMembershipTypeByIdAsync(id);
            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> MembershipTypeWithGivenNameExists(string name)
        {
            return await _context.MembershipTypes.CountAsync(x => x.Name == name) > 0;
        }
        private async Task<bool> MembershipTypeWithGivenIdExists(Guid id)
        {
            return await _context.MembershipTypes.CountAsync(x => x.IdMembershipType == id) > 0;
        }
    }
}
