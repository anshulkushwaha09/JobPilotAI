using Microsoft.Data.SqlClient;

namespace JobPilot.API.Helpers;

public static class SqlDataReaderExtensions
{
    public static string? GetNullableString(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetString(ordinal);
    }

    public static int? GetNullableInt(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetInt32(ordinal);
    }

    public static long? GetNullableLong(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetInt64(ordinal);
    }

    public static decimal? GetNullableDecimal(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDecimal(ordinal);
    }

    public static bool? GetNullableBool(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetBoolean(ordinal);
    }

    public static DateTime? GetNullableDateTime(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDateTime(ordinal);
    }

    public static double? GetNullableDouble(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDouble(ordinal);
    }

    public static Guid? GetNullableGuid(
        this SqlDataReader reader,
        string column)
    {
        int ordinal = reader.GetOrdinal(column);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetGuid(ordinal);
    }
}