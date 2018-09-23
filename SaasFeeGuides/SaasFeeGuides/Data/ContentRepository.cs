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
    public interface IContentRepository
    {
        Task<string> InsertContent(IList<Content> contents);
        Task<string> InsertContentList(IList<Content> contents);
        Task<string> InsertContent(Content activity);
        Task<IList<Content>> SelectContent(string contentId);
    }
    public class ContentRepository : DataAccessBase, IContentRepository
    {
        public ContentRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<string> InsertContentList(IList<Content> contents)
        {
            if (contents?.Any() ?? false)
            {
                foreach (var content in contents)
                {
                    await InsertContent(content);
                }

                return string.Join(",", contents.Select(c => c.Id));
            }

            return null;
        }
        public async Task<string> InsertContent(IList<Content> contents)
        {
            if (contents?.Any() ?? false)
            {
                foreach (var content in contents)
                {
                    await InsertContent(content);
                }

                return contents[0].Id;
            }

            return null;
        }
        public async Task<IList<Content>> SelectContent(string contentId)
        {
            if (string.IsNullOrEmpty(contentId))
                return new List<Content>();

            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@ContentId", contentId);
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "[Activities].[SelectContent]";
                    return await ReadListAsync(command,ReadContent);
                  
                }
            }
        }

        private Content ReadContent(SqlDataReader reader)
        {
            try
            {
                return new Content
                {
                    Id = GetString(reader, 0),
                    Value = GetString(reader, 1),             
                    Locale = GetString(reader, 2),
                    ContentType = GetString(reader, 3)

                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading ActivityLoc", ex);
            }
        }

        public async Task<string> InsertContent(Content content)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@Id", content.Id);
                    command.Parameters.AddWithValue("@Value", content.Value);
                    command.Parameters.AddWithValue("@Locale", content.Locale);
                    command.Parameters.AddWithValue("@ContentType", content.ContentType);
                    command.CommandType = CommandType.StoredProcedure;
                   
                    command.CommandText = "[Activities].[UpsertContent]";
                    await command.ExecuteNonQueryAsync();
                    return content.Id;
                }
            }
        }
    }
}
