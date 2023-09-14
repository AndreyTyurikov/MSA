using Microsoft.AspNetCore.Mvc;
using UserMS.DTO;
using UserMS.Services;

namespace UserMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<UserDTO> Get(long id)
        {
            return await _userService.GetUserById(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<UserDTO> Post([FromBody] AddUserDTO userToAdd)
        {
            return await _userService.CreateUserFromDto(userToAdd);
        }
    }
}
