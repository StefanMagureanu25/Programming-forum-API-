using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Exceptions;
using MagureanuStefan_API.Helpers.Enums;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagureanuStefan_API.Repositories
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly ClubLibraDataContext _context;
        public AnnouncementsRepository(ClubLibraDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Announcement>> GetAnnouncementsAsync()
        {
            return await _context.Announcements.ToListAsync();
        }
        public async Task<Announcement> GetAnnouncementByIdAsync(Guid id)
        {
            return await _context.Announcements.SingleOrDefaultAsync(x => x.IdAnnouncement == id);
        }
        public async Task CreateAnnouncementAsync(Announcement announcement)
        {
            if (announcement.Tags == null || announcement.ValidFrom == null ||
                announcement.ValidTo == null || announcement.Title == null ||
                announcement.Text == null || announcement.EventDate == null ||
                announcement.Tags == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.WrongFormatPut);
            }
            announcement.IdAnnouncement = Guid.NewGuid();
            bool titleExists = await TitleExists(announcement.Title);
            if (titleExists)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.TitleExistsError);
            }
            ValidationFunctions.ThrowExceptionWhenDateIsNotValid(announcement.ValidFrom, announcement.ValidTo);
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }
        public async Task<Announcement> UpdateAnnouncementAsync(Guid id, Announcement announcement)
        {
            if (announcement.Tags == null || announcement.ValidFrom == null ||
                announcement.ValidTo == null || announcement.Title == null ||
                announcement.Text == null || announcement.EventDate == null ||
                announcement.Tags == null)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.WrongFormatPut);
            }
            if (!await ExistAnnouncementAsync(id))
            {
                return null;
            }
            ValidationFunctions.ThrowExceptionWhenDateIsNotValid(announcement.ValidFrom, announcement.ValidTo);
            _context.Announcements.Update(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<Announcement> UpdatePartiallyAnnouncementAsync(Guid id, Announcement announcement)
        {
            var announcementFromDatabase = await GetAnnouncementByIdAsync(id);
            bool announcementIsChanged = false;
            if (announcementFromDatabase == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(announcement.Tags) && announcementFromDatabase.Tags != announcement.Tags)
            {
                announcementIsChanged = true;
                announcementFromDatabase.Tags = announcement.Tags;
            }
            if (!string.IsNullOrEmpty(announcement.Text) && announcementFromDatabase.Text != announcement.Text)
            {
                announcementIsChanged = true;
                announcementFromDatabase.Text = announcement.Text;
            }
            if (!string.IsNullOrEmpty(announcement.Title) && announcementFromDatabase.Title != announcement.Title)
            {
                announcementIsChanged = true;
                announcementFromDatabase.Title = announcement.Title;
            }
            if (announcement.ValidFrom != null && announcementFromDatabase.ValidFrom != announcement.ValidFrom)
            {
                announcementIsChanged = true;
                announcementFromDatabase.ValidFrom = announcement.ValidFrom;
            }
            if (announcement.ValidTo != null && announcementFromDatabase.ValidTo != announcement.ValidTo)
            {
                announcementIsChanged = true;
                announcementFromDatabase.ValidTo = announcement.ValidTo;
            }
            if (announcement.EventDate != null && announcementFromDatabase.EventDate != announcement.EventDate)
            {
                announcementIsChanged = true;
                announcementFromDatabase.EventDate = announcement.EventDate;
            }
            if (!announcementIsChanged)
            {
                throw new ModelValidationException(ErrorMessagesEnum.Announcement.ZeroUpdateToSave);
            }
            ValidationFunctions.ThrowExceptionWhenDateIsNotValid(announcementFromDatabase.ValidFrom, announcementFromDatabase.ValidTo);
            _context.Announcements.Update(announcementFromDatabase);
            await _context.SaveChangesAsync();
            return announcementFromDatabase;
        }
        public async Task<bool> DeleteAnnouncementAsync(Guid id)
        {
            if (!await ExistAnnouncementAsync(id))
            {
                return false;
            };
            Announcement announcement = await GetAnnouncementByIdAsync(id);
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> ExistAnnouncementAsync(Guid id)
        {
            return await _context.Announcements.CountAsync(a => a.IdAnnouncement == id) > 0;
        }
        private async Task<bool> TitleExists(string title)
        {
            return await _context.Announcements.CountAsync(a => a.Title == title) > 0;
        }
    }
}
