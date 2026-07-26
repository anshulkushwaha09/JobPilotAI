using System.Security.Claims;

namespace JobPilot.API.Helpers;

public static class CurrentUserHelper
{
    public static int GetUserId(
        ClaimsPrincipal user)
    {
        var claim =
            user.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
            return 0;

        return Convert.ToInt32(claim.Value);
    }

    public static int GetRoleId(
        ClaimsPrincipal user)
    {
        var claim =
            user.FindFirst(ClaimTypes.Role);

        if (claim == null)
            return 0;

        return Convert.ToInt32(claim.Value);
    }

    public static string GetEmail(
        ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value
               ?? "";
    }

    public static string GetName(
        ClaimsPrincipal user)
    {
        return user.Identity?.Name ?? "";
    }
}