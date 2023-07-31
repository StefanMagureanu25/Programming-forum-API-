using MagureanuStefan_API.Models;

namespace MagureanuStefan_API.Repositories.Interfaces
{
    public interface IAnnouncementsRepository
    {
        Task<IEnumerable<Announcement>> GetAnnouncementsAsync();
        Task<Announcement> GetAnnouncementByIdAsync(Guid id);
        Task CreateAnnouncementAsync(Announcement announcement);
        Task<Announcement> UpdateAnnouncementAsync(Guid id, Announcement announcement);
        Task<Announcement> UpdatePartiallyAnnouncementAsync(Guid id, Announcement announcement);
        Task<bool> DeleteAnnouncementAsync(Guid id);
    }
}
