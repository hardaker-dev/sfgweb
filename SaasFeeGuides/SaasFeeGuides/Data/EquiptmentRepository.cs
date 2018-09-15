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
    public interface IEquiptmentRepository
    {
        Task<int> UpsertEquiptment(Models.Equiptment equiptmentModel);
    }
    public class EquiptmentRepository : DataAccessBase, IEquiptmentRepository
    {
        public EquiptmentRepository(string connectionString) : base(connectionString)
        {
           
        }

        public async Task<int> UpsertEquiptment(Models.Equiptment equiptmentModel)
        {            
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@Name", equiptmentModel.Name);
                    command.Parameters.AddWithValue("@TitleContentId",await equiptmentModel.TitleContentId);
                    command.Parameters.AddWithValue("@RentalPrice", equiptmentModel.RentalPrice);
                    command.Parameters.AddWithValue("@ReplacementPrice", equiptmentModel.ReplacementPrice);
                    command.Parameters.AddWithValue("@CanRent", equiptmentModel.CanRent);
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "[Activities].[UpsertEquiptment]";
                    command.Parameters.AddWithValue("@Id", (object)equiptmentModel.Id ?? DBNull.Value);

                    return (int)await command.ExecuteScalarAsync();

                }
            }        
        }
    }
}
