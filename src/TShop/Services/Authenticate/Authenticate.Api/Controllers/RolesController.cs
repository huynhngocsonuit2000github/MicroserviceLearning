using Authenticate.Api.Entity;
using Authenticate.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IRoleRepository repository, ILogger<RolesController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
         
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            _logger.LogInformation("==>> Start GetRoles");
            var roles = await _repository.GetRoles();
            return Ok(roles); 
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:length(24)}", Name = "GetRole")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Role), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Role>> GetRoleById(string id)
        {
            _logger.LogInformation($"==>> Start GetRoleById: {id}");
            var role = await _repository.GetRole(id);
            if (role == null)
            {
                _logger.LogError($"==>> Role with id: {id}, not found.");
                return NotFound();
            }
            return Ok(role);
        }

        [Authorize(Roles = "Admin")]
        [Route("[action]/{name}", Name = "GetRoleByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Role>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoleByName(string name)
        {
            _logger.LogInformation($"==>> Start GetRoleByName: {name}");
            var roles = await _repository.GetRoleByName(name);
            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(Role), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
        {
            _logger.LogInformation("==>> Start CreateRole: \n" + role.ToJson()); 
            await _repository.CreateRole(role);
            return CreatedAtRoute("GetRole", new { id = role.Id }, role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(typeof(Role), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateRole([FromBody] Role role)
        {
            _logger.LogInformation("==>> Start UpdateRole: \n" + role.ToJson());
            return Ok(await _repository.UpdateRole(role));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:length(24)}", Name = "DeleteRole")]
        [ProducesResponseType(typeof(Role), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteRoleById(string id)
        {
            _logger.LogInformation("==>> Start DeleteRoleById: " + id);
            return Ok(await _repository.DeleteRole(id));
        }
    }
}
