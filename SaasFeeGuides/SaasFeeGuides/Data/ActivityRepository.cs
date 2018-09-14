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
        Task<int> UpsertActivity(Models.Activity activity);
        Task<int> FindActivityByName(string name);
        Task<int> UpsertActivitySku(Models.ActivitySku activitySku);
        Task<int> FindActivitySkuByName(string name);
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

        public async Task<int> UpsertActivity(Models.Activity activity)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@Name", activity.Name);
                        command.Parameters.AddWithValue("@TitleContentId", await activity.TitleContentId);
                        command.Parameters.AddWithValue("@DescriptionContentId",  await activity.DescriptionContentId);
                        command.Parameters.AddWithValue("@MenuImageContentId", await activity.MenuImageContentId);
                        command.Parameters.AddWithValue("@VideoContentIds", (await activity.VideoContentIds) ?? string.Empty);
                        command.Parameters.AddWithValue("@ImageContentIds", (await activity.ImageContentIds) ?? string.Empty);
                        command.Parameters.AddWithValue("@IsActive", activity.IsActive ?? false);
                        command.CommandType = CommandType.StoredProcedure;
                        if (activity.Id.HasValue)
                        {
                            command.CommandText = "[Activities].[UpdateActivity]";

                            command.Parameters.AddWithValue("@Id", activity.Id.Value);

                            await command.ExecuteNonQueryAsync();
                            return activity.Id.Value;
                        }
                        else
                        {
                            command.CommandText = "[Activities].[InsertActivity]";
                            var result = await command.ExecuteScalarAsync();
                            return (int)result;
                        }
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
                        if (activitySku.Id.HasValue)
                        {
                            command.CommandText = "[Activities].[UpdateActivitySku]";
                            command.Parameters.AddWithValue("@Id", activitySku.Id.Value);

                            await command.ExecuteNonQueryAsync();
                            return activitySku.Id.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ActivityName", activitySku.ActivityName);
                            command.CommandText = "[Activities].[InsertActivitySku]";
                            var result = await command.ExecuteScalarAsync();
                            return (int)result;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.UniqueIndexViolation:
                        throw new BadRequestException($"Activity Sku already exists with Name '{activitySku.Name}'", e);

                    default: throw e;
                }
            }
        }
    }
}
