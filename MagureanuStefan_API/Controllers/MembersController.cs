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
    public class MembersController : ControllerBase
    {
        private readonly IMembersRepository _membersRepository;
        private readonly ILogger<MembersController> _logger;
        public MembersController(IMembersRepository membersRepository, ILogger<MembersController> logger)
        {
            _membersRepository = membersRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var members = await _membersRepository.GetAllMembersAsync();
                if (members == null || members.Count() < 1)
                {
                    _logger.LogInformation("There is no member in database!!!!");
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Member.NotFound);
                }
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems with getting all the members: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var member = await _membersRepository.GetMemberByIdAsync(id);
                if (member == null)
                {
                    _logger.LogInformation($"There is no member with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Member.NotFoundById);
                }
                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There is a problem with getting the member with id {id}: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Member member)
        {
            try
            {
                await _membersRepository.CreateMemberAsync(member);
                return Created(SuccesMessagesEnum.Member.MemberAdded, member);
            }
            catch (ModelValidationException ex)
            {
                _logger.LogInformation($"{ex.Message}");
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when adding a member: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] Member member)
        {
            try
            {
                member.IdMember = id;
                var updatedMember = await _membersRepository.UpdateMemberAsync(id, member);
                if (updatedMember == null)
                {
                    _logger.LogInformation($"I couldn't find member with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Member.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Member.MemberUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] Member member)
        {
            try
            {
                if (member == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Member.BadRequest);
                }
                var updatedModel = await _membersRepository.UpdateMemberPartiallyAsync(id, member);
                if (updatedModel == null)
                {
                    _logger.LogInformation($"There is no member with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Member.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Member.MemberUpdated);
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
                var result = await _membersRepository.DeleteMemberAsync(id);
                if (!result)
                {
                    _logger.LogInformation($"I couldn't find member with id {id} for deletion.");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Member.NotFoundById);
                }
                _logger.LogInformation($"Member with id {id} was deleted successfully!");
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Member.MemberDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
