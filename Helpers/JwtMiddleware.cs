using System.IdentityModel.Tokens.Jwt;
using System.Text;
using KitchenKitApi.Data.Repositories;
using KitchenKitApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KitchenKitApi.Helpers;

public class JwtMiddleware {
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings){
        _next = next;
        _appSettings = appSettings.Value;
    }

    
    /// <summary>
    /// The method that is invoked when a http request is made that requires the user to be authorized.
    /// </summary>
    /// <param name="context">The context of the http request.</param>
    /// <param name="userRespository">An instance of the user repository.</param>
    public async Task Invoke(HttpContext context, IUserRespository userRespository) {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(context, userRespository, token);

        await _next(context);
    }

    /// <summary>
    /// The method used to attach the logged in user to the context.
    /// </summary>
    /// <param name="context">The context of the http request.</param>
    /// <param name="userRespository">An instance of the user repository.</param>
    /// <param name="token">The token of the logged in user.</param>
    private void attachUserToContext(HttpContext context, IUserRespository userRespository, string token) {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            //Attach user to context on successful JWT validation
            context.Items["User"] = userRespository.GetById(userId);
        } catch {
            //Do nothing if JWT validation fails
            // user is not attached to context so the request won't have access to secure routes
        }
    }        
}