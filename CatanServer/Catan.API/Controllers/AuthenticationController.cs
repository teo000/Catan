﻿using Catan.Application.Contracts;
using Catan.Application.Models.Identity;
using Catan.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catan.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly ILogger<AuthenticationController> _logger;

	public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
	{
		_authService = authService;
		_logger = logger;
	}

	[HttpPost]
	[Route("login")]
	public async Task<IActionResult> Login(LoginModel model)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid payload");
			}

			var (status, message) = await _authService.Login(model);

			if (status == 0)
			{
				return BadRequest(message);
			}

			return Ok(message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
		}
	}

	[HttpPost]
	[Route("register")]
	public async Task<IActionResult> Register(RegistrationModel model)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid payload");
			}

			var (status, message) = await _authService.Registration(model, UserRoles.User);

			if (status == 0)
			{
				return BadRequest(message);
			}

			return CreatedAtAction(nameof(Register), model);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
			return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
		}
	}

	[HttpPost]
	[Route("logout")]
	public async Task<IActionResult> Logout()
	{
		await _authService.Logout();
		return Ok();
	}

}
