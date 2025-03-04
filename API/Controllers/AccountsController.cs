using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
{

    [HttpPost("register")] // POST: api/accounts/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        if (string.IsNullOrEmpty(registerDto.Username))
            return BadRequest("Username cannot be null or empty.");
        
        if (string.IsNullOrEmpty(registerDto.Password))
            return BadRequest("Password cannot be null or empty.");

        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
    {
        var user = await context.Users
        .Include(u => u.Photos)
        .FirstOrDefaultAsync(x =>
        x.UserName == loginDto.Username.ToLower());

        // .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid Username");

        if (user.PasswordSalt == null)
        {
            return Unauthorized("Invalid user data. Password salt is missing.");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        if (string.IsNullOrEmpty(loginDto.Password))
        {
            return Unauthorized("Password cannot be null or empty.");
        }

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (user.PasswordHash == null)
        {
            return Unauthorized("Invalid user data. Password hash is missing.");
        }

        for(int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        
        return new UserDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
