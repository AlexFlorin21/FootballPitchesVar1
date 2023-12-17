using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Football.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = _userManager.Users.ToList();
        return Ok(users);
    }

    // GET: api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser([FromBody] CreateUserModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("User or password cannot be null.");
        }

        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            // Setează aici și alte proprietăți necesare ale user-ului, dacă este cazul
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Nu returna obiectul `User` complet, deoarece poate conține informații sensibile
            // În schimb, returnează un mesaj sau un DTO (Data Transfer Object) simplificat
            return Ok(new { message = "User created successfully", userId = user.Id });
        }

        return BadRequest(result.Errors);
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors);
    }
}
public class CreateUserModel
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    // Adaugă aici alte câmpuri necesare pentru înregistrarea utilizatorului
}