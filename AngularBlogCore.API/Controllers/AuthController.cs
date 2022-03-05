using AngularBlogCore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularBlogCore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult IsAuthenticated(AdminUser adminUser)
        {
            bool status = false;

            if (adminUser.Email == "f@outlook.com" && adminUser.Password == "1234")
            {
                status = true;
            }

            var result = new
            {
                status = status
            };
            return Ok(result);
        }
    }
}
