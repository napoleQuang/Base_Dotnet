using API.Models.Domains;
using API.Models.DTOS;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IConfiguration configuration;

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService authService;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IAuthService authService)
        {
            _logger = logger;
            this.configuration = configuration;
            this.authService = authService;
        }
        [Authorize]

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("/information")]
        public IActionResult Infomation()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);     
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            return Ok(email);

        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            try
            {
                var register = await authService.Register(model);
                if (register == null) return BadRequest();
                return Ok(register);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                var login = await authService.Login(model);
                return Ok(login);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}