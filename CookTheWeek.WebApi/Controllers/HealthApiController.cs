namespace CookTheWeek.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiController]
    [Route("api/health")]
    public class HealthApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckHealth()
        {
            return Ok("API is healthy");
        }
    }
}
