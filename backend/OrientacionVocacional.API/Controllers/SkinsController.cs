using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/student/skins")]
[Authorize(Roles = RoleNames.Student)]
public sealed class SkinsController(ISkinService skinService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<SkinShopResponse>> Get(CancellationToken cancellationToken)
    {
        var shop = await skinService.GetShopAsync(GetUserId(), cancellationToken);
        return shop is null ? NotFound() : Ok(shop);
    }

    [HttpPost("{skinId}/buy")]
    public async Task<ActionResult<SkinShopResponse>> Buy(string skinId, CancellationToken cancellationToken)
    {
        var result = await skinService.BuyAsync(GetUserId(), skinId, cancellationToken);
        return result.Success ? Ok(result.Shop) : BadRequest(new { message = result.Error });
    }

    [HttpPost("{skinId}/equip")]
    public async Task<ActionResult<SkinShopResponse>> Equip(string skinId, CancellationToken cancellationToken)
    {
        var result = await skinService.EquipAsync(GetUserId(), skinId, cancellationToken);
        return result.Success ? Ok(result.Shop) : BadRequest(new { message = result.Error });
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
