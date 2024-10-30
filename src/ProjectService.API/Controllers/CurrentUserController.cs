using Microsoft.AspNetCore.Mvc;
using ProjectService.API.Services;

namespace ProjectService.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CurrentUserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public CurrentUserController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }



        [HttpGet("user/current")]
        public ActionResult<string> Get()
        {
            return _currentUserService.GetUserId();
        }
    }


}

