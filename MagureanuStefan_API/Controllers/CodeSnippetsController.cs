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
    public class CodeSnippetsController : ControllerBase
    {
        private ILogger<CodeSnippetsController> _logger;
        private ICodeSnippetsRepository _codeSnippetsRepository;
        public CodeSnippetsController(ILogger<CodeSnippetsController> logger, ICodeSnippetsRepository codeSnippetsRepository)
        {
            _logger = logger;
            _codeSnippetsRepository = codeSnippetsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var codeSnippets = await _codeSnippetsRepository.GetAllCodeSnippetsAsync();
                if (codeSnippets == null || codeSnippets.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.CodeSnippet.NotFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.CodeSnippet.NotFound);
                }
                return Ok(codeSnippets);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get CodeSnippets error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var codeSnippet = await _codeSnippetsRepository.GetCodeSnippetByIdAsync(id);
                if (codeSnippet == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.CodeSnippet.NotFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.CodeSnippet.NotFoundById);
                }
                return Ok(codeSnippet);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get a specific CodeSnippet error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CodeSnippet codeSnippet)
        {
            try
            {
                await _codeSnippetsRepository.CreateCodeSnippetAsync(codeSnippet);
                return Created(SuccesMessagesEnum.CodeSnippet.CodeSnippetAdded, codeSnippet);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Add a new codeSnippet error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] CodeSnippet codeSnippet)
        {
            try
            {
                codeSnippet.IdCodeSnippet = id;
                var updatedCodeSnippet = await _codeSnippetsRepository.UpdateCodeSnippetAsync(id, codeSnippet);
                if (updatedCodeSnippet == null)
                {
                    _logger.LogInformation($"I couldn't find the codeSnippet with id {id} in database");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.CodeSnippet.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.CodeSnippet.CodeSnippetUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CodeSnippet database failed to update: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] CodeSnippet codeSnippet)
        {
            try
            {
                if (codeSnippet == null)
                {
                    _logger.LogInformation("Doesn't make sense to update with an empty codeSnippet");
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.CodeSnippet.BadRequest);
                }
                var updatedCodeSnippet = await _codeSnippetsRepository.UpdatePartiallyCodeSnippetAsync(id, codeSnippet);
                if (updatedCodeSnippet == null)
                {
                    _logger.LogInformation($"I couldn't find the codeSnippet with id {id} in database");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.CodeSnippet.NotFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.CodeSnippet.CodeSnippetUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CodeSnippet database failed to update: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var deletedCodeSnippet = await _codeSnippetsRepository.DeleteCodeSnippetAsync(id);
                if (deletedCodeSnippet)
                {
                    _logger.LogInformation($"CodeSnippet with id {id} was successfully deleted!!!");
                    return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.CodeSnippet.CodeSnippetDeleted);
                }
                _logger.LogInformation($"I couldn't find the codeSnippet with id {id} in database");
                return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.CodeSnippet.NotFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CodeSnippet database failed to delete: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
