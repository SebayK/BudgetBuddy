using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetBuddy.Enums;
using BudgetBuddy.Models;

namespace BudgetBuddy.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration _configuration;

    public AuthorizationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<(int, string)> Registration(RegistrationModel model, UserRole role)
    {
        var userExists = await userManager.FindByNameAsync(model.Username);
        if (userExists != null)
            return (0, "User already exists");

        User user = new()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            Role = model.Role,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        var createUserResult = await userManager.CreateAsync(user, model.Password);
        if (!createUserResult.Succeeded)
            return (0, "User creation failed! Please check user details and try again.");

        if (!await roleManager.RoleExistsAsync(role.ToString()))
            await roleManager.CreateAsync(new IdentityRole(role.ToString()));

        if (await roleManager.RoleExistsAsync(role.ToString()))
            await userManager.AddToRoleAsync(user, role.ToString());

        return (1, "User created successfully!");
    }

    public async Task<(int, string)> Login(LoginModel model)
    {
        var user = await userManager.FindByNameAsync(model.Username);
        if (user == null)
            return (0, "Invalid username");
        if (!await userManager.CheckPasswordAsync(user, model.Password))
            return (0, "Invalid password");

        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        string token = GenerateToken(authClaims);
        return (1, token);
    }


    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
        var tokenExpiryTimeInMinutes = Convert.ToInt64(_configuration["JWTKey:ExpirationInMinutes"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWTKey:Issuer"],
            Audience = _configuration["JWTKey:Audience"],
            Expires = DateTime.UtcNow.AddHours(tokenExpiryTimeInMinutes),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
