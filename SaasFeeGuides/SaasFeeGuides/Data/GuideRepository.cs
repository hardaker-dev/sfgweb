using SaasFeeGuides.Exceptions;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface IGuideRepository
    {
        Task<int> UpsertGuide(Guide guide);
        Task<Guide> SelectGuideByUserId(string userId);
        Task DeleteAccount(string userId);
        Task<IEnumerable<Guide>> SelectGuides(string emailSearch, string firstNameSearch, string lastNameSearch);
        Task<Guide> SelectGuide(int id);
    }
    public class GuideRepository : DataAccessBase, IGuideRepository
    {
        public GuideRepository(string connectionString) : base(connectionString)
        {
        }




        public async Task DeleteAccount(string userId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[DeleteGuideAccount]";

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Guide> SelectGuide(int guideId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@guideId", guideId);
                    command.CommandText = "[Activities].[SelectGuide]";

                    return await ReadSingleAsync(command, ReadGuide);
                }
            }
        }

        public async Task<IEnumerable<Guide>> SelectGuides(string emailSearch,string firstNameSearch,string lastNameSearch)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {                    
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@emailSearch", (object)emailSearch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@firstNameSearch", (object)firstNameSearch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@lastNameSearch", (object)lastNameSearch ?? DBNull.Value);
                    command.CommandText = "[Activities].[SelectGuides]";

                    return await ReadListAsync(command, ReadGuide);
                }
            }
        }

        public async Task<Guide> SelectGuideByUserId(string userId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "[Activities].[SelectGuideByUserId]";

                    return await ReadSingleAsync(command, ReadGuide);

                }
            }
        }

      

        public async Task DeleteCustomerBooking(int activitySkuDateId, string customerEmail)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@ActivitySkuDateId", activitySkuDateId);
                        command.Parameters.AddWithValue("@CustomerEmail", customerEmail);

                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandText = "[Activities].[DeleteCustomerBooking]";

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.CannotFindRecord:
                        var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                        throw new BadRequestException(error.Message, HttpStatusCode.NotFound, e);
                    default: throw e;
                }
            }
        }


        public async Task<int> UpsertGuide(Guide guide)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@FirstName", guide.FirstName);
                        command.Parameters.AddWithValue("@LastName", guide.LastName);
                        command.Parameters.AddWithValue("@Email", guide.Email);
                        command.Parameters.AddWithValue("@DateOfBirth", guide.DateOfBirth == DateTime.MinValue ? DBNull.Value : (object)guide.DateOfBirth);
                        command.Parameters.AddWithValue("@PhoneNumber", guide.PhoneNumber);
                        command.Parameters.AddWithValue("@UserId", guide.UserId);
                        command.Parameters.AddWithValue("@Address", guide.Address);
                        command.CommandType = CommandType.StoredProcedure;
                        if (guide.Id.HasValue)
                        {
                            command.CommandText = "[Activities].[UpdateGuide]";

                            command.Parameters.AddWithValue("@Id", guide.Id.Value);

                            await command.ExecuteNonQueryAsync();
                            return guide.Id.Value;
                        }
                        else
                        {
                            command.CommandText = "[Activities].[InsertGuide]";
                            var result = await command.ExecuteScalarAsync();
                            return (int)result;
                        }
                    }
                }
            }
            catch(SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.UniqueIndexViolation:
                        throw new BadRequestException($"Customer already exists with Email '{guide.Email}'",System.Net.HttpStatusCode.BadRequest, e);
                    default: throw e;
                }
            }
        }


        private Guide ReadGuide(SqlDataReader reader)
        {
            try
            {
                return new Guide
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    FirstName = GetString(reader, 1),
                    LastName = GetString(reader, 2),
                    Email = GetString(reader, 3),
                    DateOfBirth = (GetDateTime(reader, 4)).GetValueOrDefault(),
                    PhoneNumber = (GetString(reader, 5)),
                    UserId = GetString(reader, 6),
                    Address = GetString(reader, 7)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Customer", ex);
            }
        }

       
    }
}
