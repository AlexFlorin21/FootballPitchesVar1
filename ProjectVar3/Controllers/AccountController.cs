using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Football.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Password))
        {
            // Tratarea cazului în care modelul sau parola sunt nule sau goale
            return BadRequest("Invalid request.");
        }

        var user = new User { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Logica adițională după înregistrarea cu succes
            return Ok(new { message = "User registered successfully!" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            // Tratarea cazului în care modelul, numele de utilizator sau parola sunt nule sau goale
            return BadRequest("Invalid request.");
        }


        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Logica adițională după autentificarea cu succes
            return Ok(new { message = "User logged in successfully!" });
        }

        return Unauthorized();
    }
}
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    [HttpGet("admin-action")]
    public IActionResult AdminAction()
    {
        return Ok(new { message = "This is an admin action." });
    }
}
