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
    public class MembershipTypesController : ControllerBase
    {
        private readonly IMembershipTypesRepository _membershipTypesRepository;
        private readonly ILogger<MembershipTypesController> _logger;
        public MembershipTypesController(IMembershipTypesRepository membershipTypesRepository, ILogger<MembershipTypesController> logger)
        {
            _membershipTypesRepository = membershipTypesRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var membershipTypes = await _membershipTypesRepository.GetAllMembershipTypesAsync();
                if (membershipTypes == null || membershipTypes.Count() == 0)
                {
                    _logger.LogInformation("There is no membershipType in database");
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.MembershipType.NotFound);
                }
                return Ok(membershipTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problems with getting all membershipTypes: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var membershipType = await _membershipTypesRepository.GetMembershipTypeByIdAsync(id);
                if (membershipType == null)
                {
                    _logger.LogInformation($"There is no membershipType with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.MembershipType.NotFound);
                }
                return Ok(membershipType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something wrong with getting the membershipType with id {id} : {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MembershipType membershipType)
        {
            try
            {
                await _membershipTypesRepository.CreateMembershipTypeAsync(membershipType);
                return Created(SuccesMessagesEnum.MembershipType.MembershipTypeAdded, membershipType);
            }
            catch (ModelValidationException ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] MembershipType membershipType)
        {
            try
            {
                membershipType.IdMembershipType = id;
                var updatedMembershipType = await _membershipTypesRepository.UpdateMembershipTypeAsync(id, membershipType);
                if (updatedMembershipType == null)
                {
                    _logger.LogInformation($"I couldn't find membershipType with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.MembershipType.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.MembershipType.MembershipTypeUpdated);
            }
            catch (ModelValidationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] MembershipType membershipType)
        {
            try
            {
                if (membershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.MembershipType.BadRequest);
                }
                var updatedMembershipType = await _membershipTypesRepository.UpdateMembershipTypeAsync(id, membershipType);
                if (updatedMembershipType == null)
                {
                    _logger.LogInformation($"I couldn't find membershipType with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.MembershipType.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.MembershipType.MembershipTypeUpdated);
            }
            catch (ModelValidationException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                bool isDeleted = await _membershipTypesRepository.DeleteMembershipTypeAsync(id);
                if (!isDeleted)
                {
                    _logger.LogInformation($"I couldn't find membershipType with id {id}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.MembershipType.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.MembershipType.MembershipTypeDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
