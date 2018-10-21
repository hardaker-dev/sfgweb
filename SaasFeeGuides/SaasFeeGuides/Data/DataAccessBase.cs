using SaasFeeGuides.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public class DataAccessBase
    {
        protected readonly string ConnectionString;

        protected DataAccessBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Invalid connection string", nameof(connectionString));

            this.ConnectionString = connectionString;
        }

        protected async Task<SqlConnection> GetNewConnectionAsync(bool loadFromPrimaryReplica = false)
        {
            var cn = new SqlConnection( this.ConnectionString);

            if (cn.State == ConnectionState.Closed)
            {
                await cn.OpenAsync();
            }
            return cn;
        }

        protected async Task<IList<T>> ReadListAsync<T>(SqlCommand command, Func<SqlDataReader, T> readerFunc, bool cacheResult = true)
        {
            return await ReadListInternalAsync(command, readerFunc);
        }

        protected async Task<T> ReadSingleAsync<T>(SqlCommand command, Func<SqlDataReader, T> readerFunc) where T : class
        {
            var result = await ReadListAsync(command, readerFunc);
            return result.Count > 0 ? result[0] : null;
        }
        protected async Task<T> ReadSingleValueAsync<T>(SqlCommand command, Func<SqlDataReader, T> readerFunc)
        {
            var result = await ReadListAsync(command, readerFunc);
            return result.Count > 0 ? result[0] : default(T);
        }
        private async Task<IList<T>> ReadListInternalAsync<T>(SqlCommand command, Func<SqlDataReader, T> readerFunc)
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await ReadListAsync( reader, readerFunc);
            }
        }

        protected static async Task<IList<T>> ReadListAsync<T>(SqlDataReader reader, Func<SqlDataReader, T> readerFunc )
        {
            var bldr = new List<T>();

            while (await reader.ReadAsync())
            {
                bldr.Add(readerFunc(reader));
            }

            return bldr;
        }

        #region Parameter Conversion
        protected static object GetNullableParameterValue(object parameter)
        {
            return parameter ?? DBNull.Value;
        }

        protected static string ConvertToString(object readerResult)
        {
            return readerResult != DBNull.Value ? Convert.ToString(readerResult) : null;
        }

        protected static int? GetInt(SqlDataReader reader, int fieldNum)
        {
            return ModelReaderExtensions.GetInteger(reader, fieldNum);
        }

        protected static double? GetDouble(SqlDataReader reader, int fieldNum)
        {
            return ModelReaderExtensions.GetDub(reader, fieldNum);
        }

        protected static bool? GetBool(SqlDataReader reader, int fieldNum)
        {
            return ModelReaderExtensions.GetBool(reader, fieldNum);
        }

        protected static string GetString(SqlDataReader reader, int fieldNum)
        {
            return ModelReaderExtensions.GetStr(reader, fieldNum);
        }
        protected static DateTime? GetDateTime(SqlDataReader reader, int fieldNum)
        {
            return ModelReaderExtensions.GetDt(reader, fieldNum);
        }
        #endregion
    }
}
