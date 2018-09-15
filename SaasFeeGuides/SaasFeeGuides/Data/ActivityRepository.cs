using SaasFeeGuides.Exceptions;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models;
using SaasFeeGuides.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Models.ActivityLoc>> SelectActivities(string locale);
        Task<int> UpsertActivity(Models.Activity activity);
        Task<int> FindActivityByName(string name);
        Task<int> UpsertActivitySku(Models.ActivitySku activitySku);
        Task<int> FindActivitySkuByName(string name);

        Task<Models.ActivityLoc> SelectActivity(int activityId, string v);
        Task<int> InsertActivitySkuDate(ActivitySkuDate activitySkuDate);
    }
    public class ActivityRepository : DataAccessBase, IActivityRepository
    {
        public ActivityRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task<int> FindActivityByName(string name)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[FindActivityByName]";
                    command.Parameters.AddWithValue("@Name", name);

                    var result = await command.ExecuteScalarAsync();
                    return (int)result;
                }
            }
        }

        public async Task<int> FindActivitySkuByName(string name)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[FindActivitySkuByName]";
                    command.Parameters.AddWithValue("@Name", name);

                    var result = await command.ExecuteScalarAsync();
                    return (int)result;
                }
            }
        }

        public async Task<IEnumerable<Models.ActivityLoc>> SelectActivities(string locale)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivities]";
                    command.Parameters.AddWithValue("@Locale", locale);


                    return await ReadListAsync(command, ReadActivity);
                  
                }
            }
        }

        public async Task<Models.ActivityLoc> SelectActivity(int activityId, string locale)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivity]";
                    command.Parameters.AddWithValue("@ActivityId", activityId);
                    command.Parameters.AddWithValue("@Locale", locale);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var activity= ReadActivity(reader);
                        await reader.NextResultAsync();
                        activity.Skus = await ReadListAsync(reader, ReadActivitySku);
                        return activity;
                    }
                 
                }
            }
        }

        private Models.ActivitySkuLoc ReadActivitySku(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivitySkuLoc
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    ActivityName = GetString(reader, 1),
                    Name = GetString(reader,2),
                    Title = GetString(reader, 3),
                    Description = GetString(reader, 4),
                    PricePerPerson = GetDouble(reader, 5) ?? 0,
                    MinPersons = GetInt(reader, 6) ?? 0,
                    MaxPersons = GetInt(reader, 7) ?? 0,
                    AdditionalRequirements = GetString(reader, 8),
                    DurationDays = GetDouble(reader, 9) ?? 0,
                    DurationHours = GetDouble(reader, 10) ?? 0,
                    WebContent = GetString(reader, 11),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivitySku", ex);
            }
        }

        private Models.ActivityLoc ReadActivity(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivityLoc
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    Name = GetString(reader, 1),
                    Title = GetString(reader, 2),
                    Description = GetString(reader, 3),
                   MenuImage = GetString(reader,4),
                    Videos = GetString(reader,5),
                    Images = GetString(reader, 6),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Activity", ex);
            }
        }

        public async Task<int> UpsertActivity(Models.Activity activity)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@Id",(object)activity.Id ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Name", activity.Name);
                        command.Parameters.AddWithValue("@TitleContentId", await activity.TitleContentId);
                        command.Parameters.AddWithValue("@DescriptionContentId",  await activity.DescriptionContentId);
                        command.Parameters.AddWithValue("@MenuImageContentId", await activity.MenuImageContentId);
                        command.Parameters.AddWithValue("@VideoContentId", (await activity.VideoContentId) ?? string.Empty);
                        command.Parameters.AddWithValue("@ImageContentId", (await activity.ImageContentId) ?? string.Empty);
                        command.Parameters.AddWithValue("@IsActive", activity.IsActive ?? false);
                        command.CommandType = CommandType.StoredProcedure;                       
                        command.CommandText = "[Activities].[UpsertActivity]";

                        return (int) await command.ExecuteScalarAsync();              
                    }
                }
            }
            catch(SqlException e)
            {
                switch((DataError)e.Number)
                {
                    case DataError.UniqueIndexViolation:
                        throw new BadRequestException($"Activity already exists with Name '{activity.Name}'", e);

                    default: throw e;
                }
            }
        }

        public async Task<int> UpsertActivitySku(Models.ActivitySku activitySku)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@Name", activitySku.Name);
                        command.Parameters.AddWithValue("@ActivityName", activitySku.ActivityName);
                        command.Parameters.AddWithValue("@TitleContentId", await activitySku.TitleContentId);
                        command.Parameters.AddWithValue("@DescriptionContentId", await activitySku.DescriptionContentId);
                        command.Parameters.AddWithValue("@PricePerPerson", activitySku.PricePerPerson);
                        command.Parameters.AddWithValue("@MinPersons", activitySku.MinPersons);
                        command.Parameters.AddWithValue("@MaxPersons", activitySku.MaxPersons);
                        command.Parameters.AddWithValue("@AdditionalRequirementsContentId", await activitySku.AdditionalRequirementsContentId);
                        command.Parameters.AddWithValue("@DurationDays", activitySku.DurationDays);
                        command.Parameters.AddWithValue("@DurationHours", activitySku.DurationHours);
                        command.Parameters.AddWithValue("@WebContentId", await activitySku.WebContentId);
                        command.CommandType = CommandType.StoredProcedure;
                       
                        command.CommandText = "[Activities].[UpsertActivitySku]";
                        command.Parameters.AddWithValue("@Id", (object)activitySku.Id ?? DBNull.Value);

                        return (int)await command.ExecuteScalarAsync();

                    }
                }
            }
            catch (SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.UniqueIndexViolation:
                        throw new BadRequestException($"Activity Sku already exists with Name '{activitySku.Name}'", e);

                    case DataError.CannotFindRecord:
                        var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                        throw new BadRequestException(error.Message, e);
                    default: throw e;
                }
            }
        }

        public async Task<int> InsertActivitySkuDate(ActivitySkuDate activitySkuDate)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@ActivityName", activitySkuDate.ActivityName);
                        command.Parameters.AddWithValue("@ActivitySkuName", activitySkuDate.ActivitySkuName);
                        command.Parameters.AddWithValue("@DateTime", activitySkuDate.DateTime);

                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandText = "[Activities].[InsertActivitySkuDate]";

                        return (int)await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.CannotFindRecord:
                        var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                        throw new BadRequestException(error.Message, e);
                    default: throw e;
                }
            }
        }
    }
}
