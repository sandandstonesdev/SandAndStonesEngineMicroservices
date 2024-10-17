using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SandAndStones.Api.DTO;
using SandAndStones.Infrastructure.Models;
using System.Security.Claims;

namespace SandAndStones.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthorizationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAuthorizationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base()
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody]LoginDTO applicationUser)
        {

            IdentityResult result = new();

            try
            {
                ApplicationUser user = new()
                {
                    Email = applicationUser.Email,
                    UserName = applicationUser.Email,
                };

                result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (!result.Succeeded)
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error while registering: " + ex.Message);
            }

            return Ok(new { message = "Registered Successfully.", result = result });
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody]LoginDTO login)
        {

            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(login.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.Remember, false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized(new { message = "Check your login credentials and try again" });
                    }

                    var updateResult = await _userManager.UpdateAsync(user);
                }
                else
                {
                    return BadRequest(new { message = "Please check your credentials and try again. " });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while logging: " + ex.Message });
            }

            return Ok(new { message = "Login Successful." });
        }

        [HttpGet("logout"), Authorize]
        public async Task<ActionResult> LogoutUser()
        {

            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while logout: " + ex.Message });
            }

            return Ok(new { message = "You are logged out." });
        }

        [HttpGet("userInfo/{email}"), Authorize]
        public async Task<ActionResult> HomePage(string email)
        {
            ApplicationUser userInfo = await _userManager.FindByEmailAsync(email);
            if (userInfo is null)
            {
                return BadRequest(new { message = "Something went wrong, please try again." });
            }

            return Ok(new { userInfo });
        }

        [HttpGet("loggedStatus")]
        public async Task<ActionResult> CheckUser()
        {
            ApplicationUser currentuser = new();

            try
            {
                var user = HttpContext.User;
                var principals = new ClaimsPrincipal(user);
                var result = _signInManager.IsSignedIn(principals);
                if (result)
                {
                    currentuser = await _signInManager.UserManager.GetUserAsync(principals);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something went wrong please try again. " + ex.Message });
            }

            return Ok(new { message = "Logged in", user = currentuser });
        }

    }
}