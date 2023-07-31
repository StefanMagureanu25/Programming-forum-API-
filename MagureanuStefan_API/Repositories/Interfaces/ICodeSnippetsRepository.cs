using MagureanuStefan_API.Models;

namespace MagureanuStefan_API.Repositories.Interfaces
{
    public interface ICodeSnippetsRepository
    {
        Task<IEnumerable<CodeSnippet>> GetAllCodeSnippetsAsync();
        Task<CodeSnippet> GetCodeSnippetByIdAsync(Guid id);
        Task CreateCodeSnippetAsync(CodeSnippet codeSnippet);
        Task<CodeSnippet> UpdateCodeSnippetAsync(Guid id, CodeSnippet codeSnippet);
        Task<CodeSnippet> UpdatePartiallyCodeSnippetAsync(Guid id, CodeSnippet codeSnippet);
        Task<bool> DeleteCodeSnippetAsync(Guid id);
    }
}
