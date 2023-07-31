using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Exceptions;
using MagureanuStefan_API.Helpers.Enums;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MagureanuStefan_API.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ClubLibraDataContext _context;
        public MembersRepository(ClubLibraDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members.ToListAsync();
        }
        public async Task<Member> GetMemberByIdAsync(Guid id)
        {
            return _context.Members.FirstOrDefault(x => x.IdMember == id);
        }
        public async Task CreateMemberAsync(Member member)
        {
            if (member.Name == null || member.Title == null ||
              member.Position == null || member.Description == null ||
              member.Resume == null || member.Username == null ||
              member.Password == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Member.WrongFormatPut);
            }
            bool isUsernameInDb = await UsernameExists(member.Username);
            if (!isUsernameInDb)
            {
                member.IdMember = Guid.NewGuid();
                _context.Members.Add(member);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ModelValidationException(ErrorMessagesEnum.Member.UsernameExists);
            }
        }
        public async Task<Member> UpdateMemberAsync(Guid id, Member member)
        {
            if (member.Name == null || member.Title == null ||
               member.Position == null || member.Description == null ||
               member.Resume == null || member.Username == null ||
               member.Password == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Member.WrongFormatPut);
            }
            bool isUsernameInDb = await UsernameExistsBesidesItself(member.Username);
            if (isUsernameInDb)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Member.UsernameExists);
            }

            if (!await ExistsMemberWithId(id))
            {
                return null;
            }
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task<Member> UpdateMemberPartiallyAsync(Guid id, Member member)
        {
            bool isChanged = false;
            if (!await ExistsMemberWithId(id))
            {
                return null;
            }

            var memberFromDatabase = await GetMemberByIdAsync(id);
            if (!String.IsNullOrEmpty(member.Name) && memberFromDatabase.Name != member.Name)
            {
                isChanged = true;
                memberFromDatabase.Name = member.Name;
            }
            if (!String.IsNullOrEmpty(member.Title) && memberFromDatabase.Title != member.Title)
            {
                isChanged = true;
                memberFromDatabase.Title = member.Title;
            }
            if (!String.IsNullOrEmpty(member.Position) && memberFromDatabase.Position != member.Position)
            {
                isChanged = true;
                memberFromDatabase.Position = member.Position;
            }
            if (!String.IsNullOrEmpty(member.Description) && memberFromDatabase.Description != member.Description)
            {
                isChanged = true;
                memberFromDatabase.Description = member.Description;
            }
            if (!String.IsNullOrEmpty(member.Resume) && memberFromDatabase.Resume != member.Resume)
            {
                isChanged = true;
                memberFromDatabase.Resume = member.Resume;
            }
            if (!String.IsNullOrEmpty(member.Username) && memberFromDatabase.Username != member.Username)
            {
                if (!await UsernameExistsBesidesItself(member.Username))
                {
                    isChanged = true;
                    memberFromDatabase.Username = member.Username;
                }
                else
                {
                    throw new ModelValidationException(ErrorMessagesEnum.Member.UsernameExists);
                }
            }
            if (!String.IsNullOrEmpty(member.Password) && memberFromDatabase.Password != member.Password)
            {
                isChanged = true;
                memberFromDatabase.Password = member.Password;
            }
            if (!isChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Member.ZeroUpdateToSave);
            }
            _context.Members.Update(memberFromDatabase);
            await _context.SaveChangesAsync();
            return memberFromDatabase;

        }
        public async Task<bool> DeleteMemberAsync(Guid id)
        {
            if (!await ExistsMemberWithId(id))
            {
                return false;
            }
            var member = await GetMemberByIdAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> UsernameExists(string username)
        {
            return await _context.Members.CountAsync(x => x.Username == username) > 0;
        }
        private async Task<bool> UsernameExistsBesidesItself(string username)
        {
            return await _context.Members.CountAsync(x => x.Username == username) > 1;
        }
        private async Task<bool> ExistsMemberWithId(Guid id)
        {
            return await _context.Members.CountAsync(x => x.IdMember == id) > 0;
        }
    }
}
