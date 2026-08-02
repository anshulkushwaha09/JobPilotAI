using System.Data;
using JobPilot.API.Data;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace JobPilot.API.Repositories.Implementations;


public class ResumeRepository : IResumeRepository
{

    private readonly DbConnectionFactory _db;


    public ResumeRepository(
    DbConnectionFactory db)
    {
        _db = db;
    }



    public async Task<long> UploadResumeAsync(
    int userId,
    string resumeName,
    string resumeUrl,
    string fileType,
    long fileSize,
    string? resumeText,
    bool isDefault)
    {

        using SqlConnection connection = _db.CreateConnection();


        using SqlCommand command =
        new SqlCommand("sp_UploadResume", connection);


        command.CommandType = CommandType.StoredProcedure;


        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@ResumeName", resumeName);
        command.Parameters.AddWithValue("@ResumeUrl", resumeUrl);
        command.Parameters.AddWithValue("@FileType", fileType);
        command.Parameters.AddWithValue("@FileSize", fileSize);
        command.Parameters.AddWithValue("@ResumeText", (object?)resumeText ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsDefault", isDefault);


        await connection.OpenAsync();


        var result = await command.ExecuteScalarAsync();


        return Convert.ToInt64(result);

    }



    public async Task<List<ResumeResponse>> GetUserResumesAsync(
    int userId)
    {

        var list = new List<ResumeResponse>();


        using SqlConnection connection = _db.CreateConnection();


        using SqlCommand command =
        new SqlCommand("sp_GetUserResumes", connection);


        command.CommandType = CommandType.StoredProcedure;


        command.Parameters.AddWithValue("@UserId", userId);


        await connection.OpenAsync();


        using var reader = await command.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {

            list.Add(new ResumeResponse
            {

                ResumeId = Convert.ToInt64(reader["ResumeId"]),

                ResumeName = reader["ResumeName"].ToString()!,

                ResumeUrl = reader["ResumeUrl"].ToString()!,

                FileType = reader["FileType"].ToString()!,

                FileSize = Convert.ToInt64(reader["FileSize"]),

                VersionNo = Convert.ToInt32(reader["VersionNo"]),

                IsDefault = Convert.ToBoolean(reader["IsDefault"]),

                UploadedOn = Convert.ToDateTime(reader["UploadedOn"])

            });

        }


        return list;

    }




    public async Task<bool> SetDefaultResumeAsync(
    long resumeId,
    int userId)
    {

        using SqlConnection connection = _db.CreateConnection();


        using SqlCommand command =
        new SqlCommand("sp_SetDefaultResume", connection);


        command.CommandType = CommandType.StoredProcedure;


        command.Parameters.AddWithValue("@ResumeId", resumeId);
        command.Parameters.AddWithValue("@UserId", userId);


        await connection.OpenAsync();


        await command.ExecuteNonQueryAsync();


        return true;

    }





    public async Task<bool> DeleteResumeAsync(
    long resumeId,
    int userId)
    {

        using SqlConnection connection = _db.CreateConnection();


        using SqlCommand command =
        new SqlCommand("sp_DeleteResume", connection);


        command.CommandType = CommandType.StoredProcedure;


        command.Parameters.AddWithValue("@ResumeId", resumeId);
        command.Parameters.AddWithValue("@UserId", userId);


        await connection.OpenAsync();


        return await command.ExecuteNonQueryAsync() > 0;

    }



    public async Task<ResumeResponse?> GetDefaultResumeAsync(
    int userId)
    {

        var resumes = await GetUserResumesAsync(userId);


        return resumes.FirstOrDefault(x => x.IsDefault);

    }




    public async Task<ResumeResponse?> GetResumeByIdAsync(
    long resumeId,
    int userId)
    {

        using SqlConnection connection = _db.CreateConnection();


        using SqlCommand command =
        new SqlCommand("sp_GetResumeById", connection);


        command.CommandType = CommandType.StoredProcedure;


        command.Parameters.AddWithValue("@ResumeId", resumeId);
        command.Parameters.AddWithValue("@UserId", userId);


        await connection.OpenAsync();


        using var reader = await command.ExecuteReaderAsync();


        if (await reader.ReadAsync())
        {

            return new ResumeResponse
            {

                ResumeId = Convert.ToInt64(reader["ResumeId"]),

                ResumeName = reader["ResumeName"].ToString()!,

                ResumeUrl = reader["ResumeUrl"].ToString()!

            };

        }


        return null;

    }


}