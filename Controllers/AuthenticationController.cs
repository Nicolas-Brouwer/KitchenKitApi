using KitchenKitApi.Models;
using KitchenKitApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitchenKitApi.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    private AuthenticationService _authenticationService;
    
    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Gain an access token for authentication.
    ///
    /// This endpoint allows users to obtain an access token by providing their username and password.
    /// The access token can then be used for subsequent authenticated requests.
    /// </summary>
    /// <param name="requestModel">The model containing the username and password</param>
    /// <returns>Returns the access user id, email, and access token</returns>
    [HttpPost]
    [Route("token")]
    public IActionResult Login(AuthenticationRequest requestModel)
    {
        var response = _authenticationService.Authenticate(requestModel);

        if (response == null)
        {
            return BadRequest(new { message = "Email or password incorrect" });
        }

        return Ok(response);
    }
    
}