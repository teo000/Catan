﻿using Catan.Application.Contracts;
using Catan.Application.Models.Identity;
using Catan.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Catan.Identity.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> userManager;
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly SignInManager<ApplicationUser> signInManager;
	private readonly IConfiguration configuration;
	public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.configuration = configuration;
		this.signInManager = signInManager;
	}
	public async Task<(int, string)> Registration(RegistrationModel model, string role)
	{
		var userExists = await userManager.FindByNameAsync(model.Username);
		if (userExists != null)
			return (0,"User already exists");

		ApplicationUser user = new ApplicationUser()
		{
			UserName = model.Username,
			Email = model.Email,
		};
		var createUserResult = await userManager.CreateAsync(user, model.Password);
		if (!createUserResult.Succeeded)
			return (0, "User creation failed! Please check user details and try again.");

		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole(role));

		if (await roleManager.RoleExistsAsync(role))
			await userManager.AddToRoleAsync(user, role);

		return (1, "User created successfully");
	}

	public async Task<(int, string)> Login(LoginModel model)
	{
		var user = await userManager.FindByNameAsync(model.Username!);
		if (user == null)
			return (0, "Invalid username");
		if (!await userManager.CheckPasswordAsync(user, model.Password!))
			return (0, "Invalid password");

		var userRoles = await userManager.GetRolesAsync(user);
		var authClaims = new List<Claim>
		{
		   new Claim(ClaimTypes.Name, user.UserName!),
		   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		   new Claim("oid", user.Id),
		};

		foreach (var userRole in userRoles)
		{
			authClaims.Add(new Claim(ClaimTypes.Role, userRole));
		}
		string token = GenerateToken(authClaims);
		return (1, token);
	}

	public async Task<(int, string)> Logout()
	{
		await signInManager.SignOutAsync();
		return (1, "User logged out successfully!");
	}


	private string GenerateToken(IEnumerable<Claim> claims)
	{
		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = configuration["JWT:ValidIssuer"]!,
			Audience = configuration["JWT:ValidAudience"]!,
			Expires = DateTime.UtcNow.AddHours(3),
			SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
			Subject = new ClaimsIdentity(claims)
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}
