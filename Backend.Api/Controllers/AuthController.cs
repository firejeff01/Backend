using Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Api.Models.Api.Request.Auth;
using Backend.Api.Models.Api.Response.Auth;
using Swashbuckle.AspNetCore.Annotations;
using Backend.Api.ApplicationServices.Interfaces;

namespace Backend.Api.Controllers
{
    [Route("api/main/cuth")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAppService _service;

        public AuthController(IAuthAppService service)
        {
            _service = service;
        }

        [HttpPost("login"), AllowAnonymous]
        [Produces("application/json")]
        [SwaggerResponse(200, "Success", typeof(AuthResponse))]
        public async Task<IActionResult> Login([FromBody] AuthRequest req)
        {
            var token = await _service.LoginAsync(req.Email, req.Password);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" }); 
            return Ok(new { token });
        }
    }
}
