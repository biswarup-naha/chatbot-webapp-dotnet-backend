using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using platychat_dotnet.DTOs;
using platychat_dotnet.Models;

namespace platychat_dotnet.Utils;

public class Jwt
{
    public static string GenerateToken(UserDto user)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var claims = new List<Claim>
        {
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.FirstName),
            // new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim("email", user.Email),
            new Claim("chats", string.Join(",", user.Chats)),
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["JwtSettings:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}

