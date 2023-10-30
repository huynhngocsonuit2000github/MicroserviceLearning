using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;
using Users.Api.Entity;
using Users.Api.Model;
using Users.Api.Repository;

namespace Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            _logger.LogInformation("==>> Start GetUsers");
            var users = await _repository.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            _logger.LogInformation($"==>> Start GetUserById: {id}");
            var user = await _repository.GetUser(id);
            if (user == null)
            {
                _logger.LogError($"==>> User with id: {id}, not found.");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("GetUserByLoginRequest")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> GetUserByLoginRequest([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"==>> Start GetUserByUsernameAndPassword: {request.ToJson()}");
            var user = await _repository.GetUserByLoginRequest(request);
            if (user == null)
            {
                _logger.LogError($"==>> User not found.");
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            _logger.LogInformation("==>> Start CreateUser: \n" + user.ToJson());
            await _repository.CreateUser(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            _logger.LogInformation("==>> Start UpdateUser: \n" + user.ToJson());
            return Ok(await _repository.UpdateUser(user));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            _logger.LogInformation("==>> Start DeleteUserById: " + id);
            return Ok(await _repository.DeleteUser(id));
        }
    }
}
