using Microsoft.AspNetCore.Mvc;
using Backend.Shared.Security;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/debug")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DebugController : ControllerBase
    {
        private readonly IPasswordHasher _hasher;

        public DebugController(IPasswordHasher hasher)
        {
            _hasher = hasher;
        }

        public class VerifyRequest
        {
            public string StoredHash { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("verify-password")]
        public IActionResult VerifyPassword([FromBody] VerifyRequest req)
        {
            if (string.IsNullOrEmpty(req.StoredHash) || string.IsNullOrEmpty(req.Password))
                return BadRequest(new { error = "storedHash and password are required" });

            try
            {
                var ok = _hasher.Verify(req.StoredHash, req.Password);
                return Ok(new { ok });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
