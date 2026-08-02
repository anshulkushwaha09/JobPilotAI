using System.Data;
using Microsoft.Data.SqlClient;
using JobPilot.API.Data;
using JobPilot.API.DTOs.Profile;
using JobPilot.API.Repositories.Interfaces;
using JobPilot.API.Helpers;
using JobPilot.API.Repositories.Base;

namespace JobPilot.API.Repositories.Implementations;

public class ProfileRepository
    : BaseRepository,
      IProfileRepository
{
    private readonly DbConnectionFactory _db;

    public ProfileRepository(DbConnectionFactory db) : base(db)
    {
        _db = db;
    }

    public async Task<ProfileResponse?> GetProfileAsync(int userId)
    {
        using SqlConnection connection =
    CreateConnection();

        using SqlCommand command =
            CreateStoredProcedure(
                "sp_GetUserProfile",
                connection);

        AddParameter(command, "@UserId", userId);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new ProfileResponse
        {
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Email = reader.GetString(reader.GetOrdinal("Email")),

            PhoneNumber = reader.GetNullableString("PhoneNumber"),

            Experience = reader.GetNullableDecimal("Experience"),

            CurrentCompany = reader.GetNullableString("CurrentCompany"),

            CurrentDesignation = reader.GetNullableString("CurrentDesignation"),

            CurrentCTC = reader.GetNullableDecimal("CurrentCTC"),

            ExpectedCTC = reader.GetNullableDecimal("ExpectedCTC"),

            NoticePeriod = reader.GetNullableInt("NoticePeriod"),

            ResumeUrl = reader.GetNullableString("ResumeUrl"),

            LinkedInUrl = reader.GetNullableString("LinkedInUrl"),

            GitHubUrl = reader.GetNullableString("GitHubUrl"),

            PortfolioUrl = reader.GetNullableString("PortfolioUrl"),

            ProfilePictureUrl = reader.GetNullableString("ProfilePictureUrl")
        };
    }


    public async Task<bool> UpdateProfileAsync(
    int userId,
    UpdateProfileRequest request)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_UpdateUserProfile", connection);

        command.CommandType = CommandType.StoredProcedure;

        AddParameter(command, "@UserId", userId);
        AddParameter(command, "@FullName", request.FullName);
        AddParameter(command, "@PhoneNumber", request.PhoneNumber);
        AddParameter(command, "@Experience", request.Experience);
        AddParameter(command, "@CurrentCompany", request.CurrentCompany);
        AddParameter(command, "@CurrentDesignation", request.CurrentDesignation);
        AddParameter(command, "@CurrentCTC", request.CurrentCTC);
        AddParameter(command, "@ExpectedCTC", request.ExpectedCTC);
        AddParameter(command, "@NoticePeriod", request.NoticePeriod);
        AddParameter(command, "@LinkedInUrl", request.LinkedInUrl);
        AddParameter(command, "@GitHubUrl", request.GitHubUrl);
        AddParameter(command, "@PortfolioUrl", request.PortfolioUrl);

        await connection.OpenAsync();

        object? result =
            await command.ExecuteScalarAsync();

        return Convert.ToInt32(result) == 1;
    }


}