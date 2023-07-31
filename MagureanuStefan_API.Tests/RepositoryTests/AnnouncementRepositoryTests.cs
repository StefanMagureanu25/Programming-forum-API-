using MagureanuStefan_API.DataContext;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories;
using MagureanuStefan_MVC.UnitTests.Helpers;
using Newtonsoft.Json;

namespace MagureanuStefan_API.Tests.RepositoryTests
{
    public class AnnouncementRepositoryTests
    {
        private readonly AnnouncementsRepository _repository;
        private readonly ClubLibraDataContext _context;
        public AnnouncementRepositoryTests()
        {
            _context = DbContextHelper.GetDatabaseContext();
            _repository = new AnnouncementsRepository(_context);
        }
        [Fact]
        public async Task GetAllAnnouncements_ExistsAnnouncements()
        {
            // Arrange -> voi crea cateva anunturi fake in memorie
            Announcement announcement1 = CreateAnnouncement(Guid.NewGuid(), "Anunt1");
            Announcement announcement2 = CreateAnnouncement(Guid.NewGuid(), "Anunt2");
            DbContextHelper.AddAnnouncement(_context, announcement1);
            DbContextHelper.AddAnnouncement(_context, announcement2);

            //Act -> Chem metoda pe care vreau sa o testez
            var dbAnnouncements = await _repository.GetAnnouncementsAsync();

            //Assert -> Verifica rezultatul
            Assert.Equal(2, dbAnnouncements.Count());
        }

        [Fact]
        public async Task GetAllAnnouncements_WithoutDataInDatabase()
        {
            //Act
            var dbAnnouncements = await _repository.GetAnnouncementsAsync();


            //Assert
            Assert.Empty(dbAnnouncements);
        }
        [Fact]
        public async Task GetAnnouncementById_WithData()
        {
            //Arrange -> creez un anunt fals
            Announcement announcement = CreateAnnouncement(Guid.NewGuid(), "Anunt");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            var dbAnnouncement = await _repository.GetAnnouncementByIdAsync(announcement.IdAnnouncement);

            //Assert
            Assert.NotNull(dbAnnouncement);
            Assert.Equal(announcement.IdAnnouncement, dbAnnouncement.IdAnnouncement);
            Assert.Equal(announcement.Title, dbAnnouncement.Title);
            var serializedAnnouncement = JsonConvert.SerializeObject(announcement);
            var serializedDbAnnouncement = JsonConvert.SerializeObject(dbAnnouncement);
            Assert.Equal(serializedDbAnnouncement, serializedAnnouncement);
        }
        [Fact]
        public async Task GetAnnouncementById_WithoutData()
        {
            Guid id = Guid.NewGuid();

            //Act
            var dbAnnouncement = await _repository.GetAnnouncementByIdAsync(id);

            //Assert
            Assert.Null(dbAnnouncement);
        }

        [Fact]
        public async Task DeleteAnnouncement_WhenExists()
        {
            //Arrange
            Announcement announcement = CreateAnnouncement(Guid.NewGuid(), "Anunt de sters");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            bool isDeleted = await _repository.DeleteAnnouncementAsync(announcement.IdAnnouncement);
            var dbAnnouncement = await _repository.GetAnnouncementByIdAsync(announcement.IdAnnouncement);

            //Assert
            Assert.True(isDeleted);
            Assert.Null(dbAnnouncement);

        }
        [Fact]
        public async Task DeleteAnnouncement_WhenNotExists()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            bool result = await _repository.DeleteAnnouncementAsync(id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdatePartially_WhenExists()
        {
            //Arrange
            Announcement announcement = CreateAnnouncement(Guid.NewGuid(), "Anunt de patch-uit");
            DbContextHelper.AddAnnouncement(_context, announcement);

            //Act
            announcement.EventDate = DateTime.Now.Date;
            var dbAnnouncement = await _repository.UpdatePartiallyAnnouncementAsync(announcement.IdAnnouncement, announcement);

            //Assert
            Assert.Equal(announcement.EventDate, DateTime.Now.Date);
        }

        private Announcement CreateAnnouncement(Guid id, string title)
        {
            Announcement announcement = new Announcement()
            {
                IdAnnouncement = id,
                Title = title,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow,
                EventDate = DateTime.UtcNow.AddDays(1),
                Tags = "#tags",
                Text = "text"
            };
            return announcement;
        }
    }
}
