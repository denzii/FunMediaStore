using FunMediaStore.Interface;
using FunMediaStore.Model.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunMediaStore.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class Auth : ControllerBase
    {
        private readonly ILogger<Auth> _logger;
        private readonly IMockRepo _repo;
        private readonly IAuth _service;

        public Auth(ILogger<Auth> logger, IAuth service, IMockRepo repo)
        {
            _logger = logger;
            _repo = repo;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login", Name = "login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest loginData)
        {
            var user = await _repo.GetUserByEmailAsync(loginData.Email!);
            if (user == null)
            {
                return NotFound("user Not found");
            }

            var secrets = _service.CreatePasswordHash(loginData.Password!, user.PasswordSalt);

            if (_service.isAuthentic(user, secrets.passwordHash))
            {
                var token = _service.GenerateToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }

        // Below functions well, however not needed at the moment  as currently working with dummy data
        //[AllowAnonymous]
        //[HttpPost(nameof(Register), Name = nameof(Register))]
        //[ActionName(nameof(Register))]
        //public async Task<IActionResult> Register([FromBody] AuthRequest registrationData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (await _repo.UserExistsAsync(registrationData.Email))
        //    {
        //        return BadRequest("Email already exists");
        //    }

        //    var user = new Model.User
        //    {
        //        Email = registrationData.Email,
        //    };

        //    var (passwordSalt, passwordHash) = _service.CreatePasswordHash(registrationData.Password);

        //    user.PasswordHash = passwordHash;
        //    user.PasswordSalt = passwordSalt;

        //    return await _repo.AddUserAsync(user)
        //        ? Ok()
        //        : UnprocessableEntity();
        //}
    }
}
