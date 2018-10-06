using SaasFeeGuides.Exceptions;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models;
using SaasFeeGuides.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface IActivityRepository
    {
        Task<IEnumerable<Models.ActivityLoc>> SelectActivities(string locale);
        Task<IEnumerable<Models.Activity>> SelectActivities();

        Task<Models.ActivityLoc> SelectActivity(int activityId, string locale);
        Task<Models.Activity> SelectActivity(int activityId);

        Task<Models.ActivitySkuLoc> SelectActivitySku(int activitySkuId, string locale);
        Task<IEnumerable<Models.ActivityDate>> SelectActivityDates(IEnumerable<int> activityIds, DateTime? dateFrom, DateTime? dateTo);

        Task<int> UpsertActivity(Models.Activity activity);       
        Task<int> UpsertActivitySku(Models.ActivitySku activitySku);

        Task<int> FindActivityByName(string name);
        Task<int> FindActivitySkuByName(string name);

        Task<int> InsertActivitySkuDate(ActivitySkuDate activitySkuDate);
        Task DeleteActivitySkuDate(int activitySkuDateId);
    }
    public class ActivityRepository : DataAccessBase, IActivityRepository
    {
        public ActivityRepository(string connectionString) : base(connectionString)
        {
           
        }


        public async Task<Models.Activity> SelectActivity(int activityId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivity]";
                    command.Parameters.AddWithValue("@ActivityId", activityId);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var activity = ReadActivity(reader);
                        await reader.NextResultAsync();
                        activity.Skus = await ReadListAsync(reader, ReadActivitySku);
                        await reader.NextResultAsync();
                        activity.Equiptment = await ReadListAsync(reader, ReadActivityEquiptment);
                        return activity;
                    }

                }
            }
        }

        public async Task<Models.ActivitySkuLoc> SelectActivitySku(int activitySkuId, string locale)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivitySku]";
                    command.Parameters.AddWithValue("@ActivitySkuId", activitySkuId);
                    command.Parameters.AddWithValue("@Locale", locale);

                    return await ReadSingleAsync(command, ReadActivitySkuLoc);
                }
            }
        }

        public async Task<IEnumerable<Models.ActivityDate>> SelectActivityDates(IEnumerable<int> activityIds, DateTime? dateFrom, DateTime? dateTo)
        {
            IList< Models.ActivityDate> results = new List<Models.ActivityDate>();
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivityDates]";
                    command.Parameters.AddWithValue("@activityIds", string.Join(',', activityIds));
                    command.Parameters.AddWithValue("@DateFrom",(object) dateFrom ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateTo", (object)dateTo ?? DBNull.Value);

                    return await ReadListAsync(command, ReadActivityDate);

                }
            }
        }

        private Models.ActivityDate ReadActivityDate(SqlDataReader reader)
        {
            return new Models.ActivityDate()
            {
                ActivitySkuDateId = GetInt(reader, 0).Value,
                ActivitySkuId = GetInt(reader, 1).Value,
                ActivityId = GetInt(reader, 2).Value,
                ActivityName = GetString(reader, 3),
                ActivitySkuName = GetString(reader, 4),
                StartDateTime = GetDateTime(reader, 5).Value,
                EndDateTime = GetDateTime(reader, 6).Value,
                NumPersons = GetInt(reader,7) ?? 0,
                TotalPrice = GetDouble(reader, 8) ?? 0,
                AmountPaid = GetDouble(reader, 9) ?? 0,
            };
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
                    if (result == null)
                        throw new BadRequestException($"Activity Sku:{name} not found", HttpStatusCode.NotFound);
                    return (int)result;
                }
            }
        }
        public async Task<IEnumerable<Models.Activity>> SelectActivities()
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[Activities].[SelectActivities]";


                    return await ReadListAsync(command, ReadActivity);

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
                    command.CommandText = "[Activities].[SelectActivitiesLoc]";
                    command.Parameters.AddWithValue("@Locale", locale);


                    return await ReadListAsync(command, ReadActivityLoc);
                  
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
                    command.CommandText = "[Activities].[SelectActivityLoc]";
                    command.Parameters.AddWithValue("@ActivityId", activityId);
                    command.Parameters.AddWithValue("@Locale", locale);


                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var activity= ReadActivityLoc(reader);
                        await reader.NextResultAsync();
                        activity.Skus = await ReadListAsync(reader, ReadActivitySkuLoc);
                        await reader.NextResultAsync();
                        activity.Equiptment = await ReadListAsync(reader, ReadEquiptmentLoc);
                        return activity;
                    }
                 
                }
            }
        }

        private Models.ActivityEquiptment ReadActivityEquiptment(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivityEquiptment
                {
                    ActivityId = GetInt(reader, 0).GetValueOrDefault(),
                    EquiptmentId = GetInt(reader, 1).GetValueOrDefault(),
                    Count = GetInt(reader, 2).GetValueOrDefault(),
                    GuideOnly = GetBool(reader, 3) ?? false,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivityEquiptment", ex);
            }
        }
        private Models.EquiptmentLoc ReadEquiptmentLoc(SqlDataReader reader)
        {
            try
            {
                return new Models.EquiptmentLoc
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),                  
                    Name = GetString(reader, 1),
                    Title = GetString(reader, 2),
                    RentalPrice = GetDouble(reader, 3) ?? 0,
                    CanRent = GetBool(reader, 4) ?? false                    
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Equiptment", ex);
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
                        command.Parameters.AddWithValue("@TitleContentId", activity.TitleContentId);
                        command.Parameters.AddWithValue("@DescriptionContentId",  activity.DescriptionContentId);
                        command.Parameters.AddWithValue("@MenuImageContentId", activity.MenuImageContentId);
                        command.Parameters.AddWithValue("@VideoContentId", activity.VideoContentId ?? string.Empty);
                        command.Parameters.AddWithValue("@ImageContentId", activity.ImageContentId ?? string.Empty);
                        command.Parameters.AddWithValue("@IsActive", activity.IsActive );
                        command.Parameters.AddWithValue("@CategoryName", activity.CategoryName );
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
                        throw new BadRequestException($"Activity already exists with Name '{activity.Name}'",HttpStatusCode.BadRequest, e);
                    case DataError.CannotFindRecord:
                        var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                        throw new BadRequestException(error.Message,HttpStatusCode.NotFound, e);
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
                        command.Parameters.AddWithValue("@TitleContentId", activitySku.TitleContentId);
                        command.Parameters.AddWithValue("@DescriptionContentId", activitySku.DescriptionContentId);
                        command.Parameters.AddWithValue("@PricePerPerson", activitySku.PricePerPerson);
                        command.Parameters.AddWithValue("@MinPersons", activitySku.MinPersons);
                        command.Parameters.AddWithValue("@MaxPersons", activitySku.MaxPersons);
                        command.Parameters.AddWithValue("@AdditionalRequirementsContentId", activitySku.AdditionalRequirementsContentId);
                        command.Parameters.AddWithValue("@DurationDays", activitySku.DurationDays);
                        command.Parameters.AddWithValue("@DurationHours", activitySku.DurationHours);
                        command.Parameters.AddWithValue("@WebContentId", activitySku.WebContentId);
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
                    case DataError.CannotFindRecord:
                        var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                        throw new BadRequestException(error.Message,HttpStatusCode.NotFound, e);
                    default: throw e;
                }
            }
        }
        public async Task DeleteActivitySkuDate(int activitySkuDateId)
        {
            try
            {
                using (var cn = await GetNewConnectionAsync())
                {
                    using (var command = cn.CreateCommand())
                    {
                        command.Parameters.AddWithValue("@activitySkuDateId", activitySkuDateId);

                        command.CommandType = CommandType.StoredProcedure;

                        command.CommandText = "[Activities].[DeleteActivitySkuDate]";

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.CannotFindRecord:
                        {
                            var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.CannotFindRecord);
                            throw new BadRequestException(error.Message, HttpStatusCode.NotFound, e);
                        }
                    case DataError.ResourceConflict:
                        {
                            var error = e.Errors.Cast<SqlError>().FirstOrDefault(ee => ee.Number == (int)DataError.ResourceConflict);
                            throw new BadRequestException(error.Message, HttpStatusCode.Conflict, e);
                        }
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
                        throw new BadRequestException(error.Message,HttpStatusCode.NotFound, e);
                    default: throw e;
                }
            }
        }
        private Models.ActivitySku ReadActivitySku(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivitySku
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    ActivityName = GetString(reader, 1),
                    Name = GetString(reader, 2),
                    TitleContentId = GetString(reader, 3),
                    DescriptionContentId = GetString(reader, 4),
                    PricePerPerson = GetDouble(reader, 5) ?? 0,
                    MinPersons = GetInt(reader, 6) ?? 0,
                    MaxPersons = GetInt(reader, 7) ?? 0,
                    AdditionalRequirementsContentId = GetString(reader, 8),
                    DurationDays = GetDouble(reader, 9) ?? 0,
                    DurationHours = GetDouble(reader, 10) ?? 0,
                    WebContentId = GetString(reader, 11),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivitySku", ex);
            }
        }
        private Models.ActivitySkuLoc ReadActivitySkuLoc(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivitySkuLoc
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    ActivityName = GetString(reader, 1),
                    Name = GetString(reader, 2),
                    Title = GetString(reader, 3),
                    Description = GetString(reader, 4),
                    PricePerPerson = GetDouble(reader, 5) ?? 0,
                    MinPersons = GetInt(reader, 6) ?? 0,
                    MaxPersons = GetInt(reader, 7) ?? 0,
                    AdditionalRequirements = GetString(reader, 8),
                    DurationDays = GetDouble(reader, 9) ?? 0,
                    DurationHours = GetDouble(reader, 10) ?? 0,
                    WebContent = GetString(reader, 11)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivitySkuLoc", ex);
            }
        }
        private Models.Activity ReadActivity(SqlDataReader reader)
        {
            try
            {
                return new Models.Activity
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    CategoryName = GetString(reader,1),
                    Name = GetString(reader, 2),
                    TitleContentId = GetString(reader, 3),
                    DescriptionContentId = GetString(reader, 4),
                    MenuImageContentId = GetString(reader, 5),
                    VideoContentId = GetString(reader, 6),
                    ImageContentId = GetString(reader, 7),
                    IsActive = GetBool(reader,8) ?? false
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Activity", ex);
            }
        }
        private Models.ActivityLoc ReadActivityLoc(SqlDataReader reader)
        {
            try
            {
                return new Models.ActivityLoc
                {
                    Id = (GetInt(reader, 0)).GetValueOrDefault(),
                    Name = GetString(reader, 1),
                    Title = GetString(reader, 2),
                    Description = GetString(reader, 3),
                    MenuImage = GetString(reader, 4),
                    Videos = GetString(reader, 5),
                    Images = GetString(reader, 6),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivityLoc", ex);
            }
        }

       
    }
}
