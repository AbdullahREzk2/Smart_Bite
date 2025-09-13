// Business/Services/JwtService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SmartBite.BAL.JWTHelper;

public class JwtService: IJWTService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _DuratuioninDays;

    public JwtService(IConfiguration configuration)
    {
        _key = configuration["JWT:Key"] ?? throw new ArgumentNullException("JWT:Key");
        _issuer = configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT:Issuer");
        _audience = configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT:Audience");
        _DuratuioninDays = int.Parse(configuration["JWT:DuratuioninDays"] ?? "30");
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_DuratuioninDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}