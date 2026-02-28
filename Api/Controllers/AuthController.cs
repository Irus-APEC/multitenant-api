using Api.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _users;
    private readonly ITokenGenerator _token;

    public AuthController(IUserService users, ITokenGenerator token)
    {
        _users = users;
        _token = token;
    }

    
    [HttpPost("/Login")]
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        var (ok, role, tenantId) = _users.ValidateCredentials(dto.Username, dto.Password);
        if (!ok) return Unauthorized(new { message = "Credenciales inválidas" });

        var user = new User { Username = dto.Username, Role = role!, TenantId = tenantId };
        var jwt = _token.CreateToken(user);

        return Ok(new { token = jwt });
    }

    
   [Authorize]
[HttpPost("CambioDeClave")]
public IActionResult CambioDeClave([FromBody] ChangePasswordDto dto)
{
  
    var tokenUsername = User.Identity?.Name;

    if (string.IsNullOrWhiteSpace(tokenUsername))
        return Unauthorized();

  
    if (tokenUsername != dto.Username)
        return Forbid();

    var ok = _users.ChangePassword(dto.Username, dto.CurrentPassword, dto.NewPassword);

    if (!ok)
        return BadRequest(new { message = "Contraseña actual incorrecta" });

    return Ok(new { message = "Contraseña actualizada correctamente" });
}

    
  [HttpPost("OlvideMiClave")]
public IActionResult OlvideMiClave([FromBody] ForgotPasswordDto dto)
{
    if (dto is null || string.IsNullOrWhiteSpace(dto.EmailOrUsername))
        return BadRequest(new { message = "Debe enviar un correo o nombre de usuario" });

    var ok = _users.ForgotPassword(dto.EmailOrUsername, out var code);

    if (!ok)
        return BadRequest(new { message = "No se pudo procesar la solicitud" });

    
    return Ok(new { message = "Solicitud registrada", resetCodeSimulado = code });
}
}