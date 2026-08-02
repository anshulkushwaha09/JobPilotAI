using System.Security.Claims;

namespace JobPilot.API.Helpers;

public static class UserContext
{
    public static int GetUserId(
        ClaimsPrincipal user)
    {
        return Convert.ToInt32(
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }

    public static string GetEmail(
        ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value ?? "";
    }

    public static int GetRoleId(
        ClaimsPrincipal user)
    {
        return Convert.ToInt32(
            user.FindFirst("RoleId")?.Value);
    }
}