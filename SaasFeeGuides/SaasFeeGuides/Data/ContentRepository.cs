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
        Task<string> InsertContent(Content[] contents);
        Task<string> InsertContentList(Content[] contents);
        Task<string> InsertContent(Content activity);
    }
    public class ContentRepository : DataAccessBase, IContentRepository
    {
        public ContentRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<string> InsertContentList(Content[] contents)
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
        public async Task<string> InsertContent(Content[] contents)
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
