using System.Reflection;

namespace JobPilot.API.Helpers;

public static class EmailTemplateHelper
{
    public static string LoadTemplate(string templatePath)
    {
        return File.ReadAllText(templatePath);
    }

    public static string ReplacePlaceholders(
        string html,
        Dictionary<string, string> values)
    {
        foreach (var item in values)
        {
            html = html.Replace($"{{{{{item.Key}}}}}", item.Value);
        }

        return html;
    }
}