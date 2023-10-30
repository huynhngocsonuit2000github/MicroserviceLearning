using Authenticate.Api.Entity;
using Authenticate.Api.Model;
using Authenticate.Api.Repository;
using Authenticate.Api.SyncData;
using Catalog.Api.Controllers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Authenticate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleRepository _repository;
        private readonly IUserproGrpc _userproGrpc;

        public AuthenticateController(ILogger<RolesController> logger, IRoleRepository repository, IUserproGrpc userproGrpc)
        {
            _logger = logger;
            _repository = repository;
            _userproGrpc = userproGrpc;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(Role), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var client = await _userproGrpc.Login(request); 
            return Ok(client);
        }
    }
}
