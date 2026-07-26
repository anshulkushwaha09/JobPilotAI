using System.Data;
using JobPilot.API.Data;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Models;
using JobPilot.API.Models.Entities;
using JobPilot.API.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace JobPilot.API.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly DbConnectionFactory _db;

    public AuthRepository(DbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<AuthResponse?> RegisterAsync(
       string fullName,
       string email,
       string passwordHash,
       int roleId)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_RegisterUser", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@FullName", fullName);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
        command.Parameters.AddWithValue("@AuthProvider", "Local");
        command.Parameters.AddWithValue("@RoleId", roleId);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new AuthResponse
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = reader["FullName"].ToString()!,
            Email = reader["Email"].ToString()!,
            RoleId = Convert.ToInt32(reader["RoleId"])
        };
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_GetUserByEmail", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Email", email);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = reader["FullName"].ToString()!,
            Email = reader["Email"].ToString()!,
            PasswordHash = reader["PasswordHash"].ToString()!,
            RoleId = Convert.ToInt32(reader["RoleId"]),
            IsActive = Convert.ToBoolean(reader["IsActive"])
        };
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_UpdateLastLogin", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }


    public async Task<User?> GetUserByGoogleIdAsync(string googleId)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_GetUserByGoogleId", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@GoogleId",
            SqlDbType.NVarChar, 250).Value = googleId;

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = reader["FullName"].ToString()!,
            Email = reader["Email"].ToString()!,
            PasswordHash = reader["PasswordHash"]?.ToString(),
            RoleId = Convert.ToInt32(reader["RoleId"]),
            GoogleId = reader["GoogleId"]?.ToString(),
            AuthProvider = reader["AuthProvider"].ToString()!,
            IsActive = Convert.ToBoolean(reader["IsActive"])
        };
    }

    public async Task<AuthResponse?> RegisterGoogleUserAsync(
    GoogleUserResponse googleUser)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_RegisterGoogleUser", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@FullName", googleUser.FullName);
        command.Parameters.AddWithValue("@Email", googleUser.Email);
        command.Parameters.AddWithValue("@GoogleId", googleUser.GoogleId);
        command.Parameters.AddWithValue("@PictureUrl", googleUser.Picture);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new AuthResponse
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = googleUser.FullName,
            Email = googleUser.Email
        };
    }


    public async Task LinkGoogleAccountAsync(
    int userId,
    string googleId,
    string pictureUrl)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_LinkGoogleAccount", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@GoogleId", googleId);
        command.Parameters.AddWithValue("@PictureUrl", pictureUrl);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }

    public async Task SaveRefreshTokenAsync(
    int userId,
    string token,
    DateTime expiry)
    {
        using SqlConnection connection =
            _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_SaveRefreshToken", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Token", token);
        command.Parameters.AddWithValue("@ExpiryDate", expiry);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }


    public async Task<RefreshToken?> GetRefreshTokenAsync(
    string token)
    {
        using SqlConnection connection =
            _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_GetRefreshToken", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Token", token);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new RefreshToken
        {
            TokenId = Convert.ToInt64(reader["TokenId"]),
            UserId = Convert.ToInt32(reader["UserId"]),
            Token = reader["Token"].ToString()!,
            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
            IsRevoked = Convert.ToBoolean(reader["IsRevoked"]),
            CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
        };
    }


    public async Task RevokeRefreshTokenAsync(
    string token)
    {
        using SqlConnection connection =
            _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_RevokeRefreshToken", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Token", token);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }



    public async Task<User?> GetUserByIdAsync(int userId)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_GetUserById", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            FullName = reader["FullName"].ToString()!,
            Email = reader["Email"].ToString()!,
            PasswordHash = reader["PasswordHash"].ToString()!,
            RoleId = Convert.ToInt32(reader["RoleId"]),
            GoogleId = reader["GoogleId"]?.ToString(),
            AuthProvider = reader["AuthProvider"]?.ToString(),
            IsActive = Convert.ToBoolean(reader["IsActive"])
        };
    }



    public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_IsRefreshTokenValid", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Token", refreshToken);

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        if (result == null)
            return false;

        return Convert.ToBoolean(result);
    }


    public async Task<bool> ChangePasswordAsync(
    int userId,
    string passwordHash)
    {
        using SqlConnection connection =
            _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_ChangePassword", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

        await connection.OpenAsync();

        object? result = await command.ExecuteScalarAsync();

        return Convert.ToBoolean(result);
    }



    public async Task SavePasswordResetTokenAsync(
    int userId,
    string token,
    DateTime expiry)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_SavePasswordResetToken", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Token", token);
        command.Parameters.AddWithValue("@ExpiryDate", expiry);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }


    public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(
    string token)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_GetPasswordResetToken", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Token", token);

        await connection.OpenAsync();

        using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new PasswordResetToken
        {
            ResetId = Convert.ToInt64(reader["ResetId"]),
            UserId = Convert.ToInt32(reader["UserId"]),
            Token = reader["Token"].ToString()!,
            ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
            IsUsed = Convert.ToBoolean(reader["IsUsed"]),
            CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
        };
    }


    public async Task MarkPasswordResetTokenUsedAsync(
    string token)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_MarkPasswordResetTokenUsed", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Token", token);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }


    public async Task UpdatePasswordAsync(
    int userId,
    string passwordHash)
    {
        using SqlConnection connection = _db.CreateConnection();

        using SqlCommand command =
            new SqlCommand("sp_UpdatePassword", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

        await connection.OpenAsync();

        await command.ExecuteNonQueryAsync();
    }



}