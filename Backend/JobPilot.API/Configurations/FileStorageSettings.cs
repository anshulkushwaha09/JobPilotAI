namespace JobPilot.API.Configurations;

public class FileStorageSettings
{
    public string ResumePath { get; set; } = string.Empty;

    public int MaxResumeSizeMB { get; set; }

    public List<string> AllowedExtensions { get; set; } = new();
}