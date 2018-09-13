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

        private async Task<IList<T>> ReadListInternalAsync<T>(SqlCommand command, Func<SqlDataReader, T> readerFunc)
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                var bldr = new List<T>();

                while (await reader.ReadAsync())
                {
                    bldr.Add(readerFunc(reader));
                }

                return bldr;
            }
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
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<int>(fieldNum);
        }

        protected static double? GetDouble(SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<double>(fieldNum);
        }

        protected static bool? GetBool(SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<bool>(fieldNum);
        }

        protected static string GetString(SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<string>(fieldNum);
        }
        protected static DateTime? GetDateTime(SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<DateTime>(fieldNum);
        }
        #endregion
    }
}
