using MagureanuStefan_API.Exceptions;
using MagureanuStefan_API.Helpers.Enums;
using MagureanuStefan_API.Models;
using MagureanuStefan_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagureanuStefan_API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private IAnnouncementsRepository _announcementsRepository;
        private ILogger<AnnouncementsController> _logger;
        public AnnouncementsController(IAnnouncementsRepository announcementsRepository, ILogger<AnnouncementsController> logger)
        {
            _announcementsRepository = announcementsRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogWarning("GetAnnouncements started");
            try
            {
                var announcements = await _announcementsRepository.GetAnnouncementsAsync();
                if (announcements == null || announcements.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NotFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Announcement.NotFound);
                }
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAnnouncements error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var announcement = await _announcementsRepository.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NotFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NotFoundById);
                }
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Announcement announcement)
        {
            try
            {
                await _announcementsRepository.CreateAnnouncementAsync(announcement);
                return Ok(SuccesMessagesEnum.Announcement.AnnouncementAdded);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create announcement error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] Announcement announcement)
        {
            try
            {
                announcement.IdAnnouncement = id;
                var updatedAnnouncement = await _announcementsRepository.UpdateAnnouncementAsync(id, announcement);
                if (updatedAnnouncement == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (ModelValidationException ex)
            {
                _logger.LogInformation("Data introduced isn't good for PUT format");
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Update announcement error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] Announcement announcement)
        {
            try
            {
                if (announcement == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                var updatedAnnouncement = await _announcementsRepository.UpdatePartiallyAnnouncementAsync(id, announcement);
                if (updatedAnnouncement == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _announcementsRepository.DeleteAnnouncementAsync(id);
                if (result)
                {
                    _logger.LogInformation($"A fost sters anuntul cu id-ul {id}");
                    return Ok(SuccesMessagesEnum.Announcement.AnnouncementDeleted);
                }
                _logger.LogInformation($"Anuntul cu id-ul {id} nu a fost gasit pentru a fi sters!");
                return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.NotFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete announcement error {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
