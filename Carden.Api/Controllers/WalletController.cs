using System.Security.Claims;
using Asp.Versioning;
using Carden.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]s")]
public class WalletController(IWalletService walletService): ControllerBase
{
    private readonly IWalletService _walletService = walletService;

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        var userId = User.FindFirst("sub")?.Value;
        var result = await _walletService.CreateAsync();
        return result.ToCreatedResult();
    }

    [HttpGet("{user_id:guid}")]
    public async Task<IActionResult> GetSingleWallet()
    {
        var userId = User.FindFirst("sub")?.Value;
        var result = await _walletService.FindAsync();
        return result.ToActionResult();
    } 
}