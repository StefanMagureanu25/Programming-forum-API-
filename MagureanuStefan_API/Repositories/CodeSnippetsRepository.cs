using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Exceptions;
using MagureanuStefan_API.Helpers.Enums;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagureanuStefan_API.Repositories
{
    public class CodeSnippetsRepository : ICodeSnippetsRepository
    {
        private readonly ClubLibraDataContext _context;
        public CodeSnippetsRepository(ClubLibraDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CodeSnippet>> GetAllCodeSnippetsAsync()
        {
            return await _context.CodeSnippets.ToListAsync();
        }
        public async Task<CodeSnippet> GetCodeSnippetByIdAsync(Guid id)
        {
            return await _context.CodeSnippets.SingleOrDefaultAsync(x => x.IdCodeSnippet == id);
        }
        public async Task CreateCodeSnippetAsync(CodeSnippet codeSnippet)
        {
            if (codeSnippet.Title == null || codeSnippet.ContentCode == null ||
                codeSnippet.IdMember == null || codeSnippet.Revision == null ||
                codeSnippet.DateTimeAdded == null || codeSnippet.IsPublished == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.WrongFormatPut);
            }
            codeSnippet.IdCodeSnippet = Guid.NewGuid();
            bool test = await MemberWithIdExists(codeSnippet.IdMember);
            if (!await MemberWithIdExists(codeSnippet.IdMember))
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.MemberDoesntExist);
            }

            bool IsContentCode = await ContentCodeExists(codeSnippet.ContentCode);
            if (IsContentCode)
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.ContentCodeError);
            }
            _context.CodeSnippets.Add(codeSnippet);
            await _context.SaveChangesAsync();
        }
        public async Task<CodeSnippet> UpdateCodeSnippetAsync(Guid id, CodeSnippet codeSnippet)
        {
            if (codeSnippet.Title == null || codeSnippet.ContentCode == null ||
                codeSnippet.IdMember == null || codeSnippet.Revision == null ||
                codeSnippet.DateTimeAdded == null || codeSnippet.IsPublished == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.WrongFormatPut);
            }
            if (!await ExistCodeSnippetAsync(id))
            {
                return null;
            }
            bool IsContentCode = await ContentCodeExists(codeSnippet.ContentCode);
            if (IsContentCode)
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.ContentCodeError);
            }
            if (!await MemberWithIdExists(codeSnippet.IdMember))
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.MemberDoesntExist);
            }
            _context.CodeSnippets.Update(codeSnippet);
            await _context.SaveChangesAsync();
            return codeSnippet;
        }
        public async Task<CodeSnippet> UpdatePartiallyCodeSnippetAsync(Guid id, CodeSnippet codeSnippet)
        {
            if (!await ExistCodeSnippetAsync(id))
            {
                return null;
            }
            var codeSnippetFromDatabase = await GetCodeSnippetByIdAsync(id);
            bool codeSnippetIsChanged = false;
            if (!String.IsNullOrEmpty(codeSnippet.Title) && codeSnippet.Title != codeSnippetFromDatabase.Title)
            {
                codeSnippetIsChanged = true;
                codeSnippetFromDatabase.Title = codeSnippet.Title;
            }
            if (!String.IsNullOrEmpty(codeSnippet.ContentCode) && codeSnippet.ContentCode != codeSnippetFromDatabase.ContentCode)
            {
                bool IsContentCode = await ContentCodeExists(codeSnippet.ContentCode);
                if (IsContentCode)
                {
                    throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.ContentCodeError);
                }
                codeSnippetIsChanged = true;
                codeSnippetFromDatabase.ContentCode = codeSnippet.ContentCode;
            }
            if (!String.IsNullOrEmpty(codeSnippet.IdMember.ToString()) && codeSnippet.IdMember != codeSnippetFromDatabase.IdMember)
            {
                if (await MemberWithIdExists(codeSnippet.IdMember))
                {
                    codeSnippetIsChanged = true;
                    codeSnippetFromDatabase.IdMember = codeSnippet.IdMember;
                }
                else
                {
                    throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.MemberDoesntExist);
                }
            }
            if (codeSnippet.Revision != codeSnippet.Revision)
            {
                codeSnippetIsChanged = true;
                codeSnippetFromDatabase.Revision = codeSnippet.Revision;
            }
            if (codeSnippet.DateTimeAdded != null && codeSnippet.DateTimeAdded != codeSnippetFromDatabase.DateTimeAdded)
            {
                codeSnippetIsChanged = true;
                codeSnippetFromDatabase.DateTimeAdded = codeSnippet.DateTimeAdded;
            }
            if (codeSnippet.IsPublished != null && codeSnippet.IsPublished != codeSnippetFromDatabase.IsPublished)
            {
                codeSnippetIsChanged = true;
                codeSnippetFromDatabase.IsPublished = codeSnippet.IsPublished;
            }
            if (!codeSnippetIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.CodeSnippet.ZeroUpdateToSave);
            }
            _context.CodeSnippets.Update(codeSnippetFromDatabase);
            await _context.SaveChangesAsync();
            return codeSnippetFromDatabase;
        }
        public async Task<bool> DeleteCodeSnippetAsync(Guid id)
        {
            if (!await ExistCodeSnippetAsync(id))
            {
                return false;
            }
            var codeSnippet = await GetCodeSnippetByIdAsync(id);
            _context.CodeSnippets.Remove(codeSnippet);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> ExistCodeSnippetAsync(Guid id)
        {
            return await _context.CodeSnippets.CountAsync(a => a.IdCodeSnippet == id) > 0;
        }
        private async Task<bool> ContentCodeExists(string contentCode)
        {
            return await _context.CodeSnippets.CountAsync(x => x.ContentCode == contentCode) > 0;
        }
        private async Task<bool> MemberWithIdExists(Guid? Id)
        {
            return await _context.Members.CountAsync(x => x.IdMember == Id) > 0;
        }
    }
}
