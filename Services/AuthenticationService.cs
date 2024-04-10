using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KitchenKitApi.Data.Repositories;
using KitchenKitApi.Helpers;
using KitchenKitApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KitchenKitApi.Services;

public class AuthenticationService
{
    private readonly AppSettings _appSettings;
    private readonly IUserRespository _userRespository;

    public AuthenticationService(IOptions<AppSettings> appSettings, IUserRespository userRespository)
    {
        _appSettings = appSettings.Value;
        _userRespository = userRespository;
    }

    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="requestModel">The model containing the login credentials of a user.</param>
    /// <returns>Returns an authentication response on success.</returns>
    public AuthenticationResponse Authenticate(AuthenticationRequest requestModel)
    {
        var user = _userRespository.GetUserByEmail(requestModel.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(requestModel.Password, user.Password)) return null;

        var token = GenerateJwtToken(user);

        return new AuthenticationResponse(user, token);
    }
    
    /// <summary>
    /// Generates a JWT token the user can use to authenticate their requests with.
    /// </summary>
    /// <param name="user">The user for which the token is generated.</param>
    /// <returns>Returns the token.</returns>
    private string GenerateJwtToken(User user) 
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}