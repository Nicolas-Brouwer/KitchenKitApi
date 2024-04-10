namespace KitchenKitApi.Models;

public class AuthenticationResponse {
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }


    public AuthenticationResponse(User user, string token){
        Id = user.Id;
        Email = user.Email;
        Token = token;
    }
}