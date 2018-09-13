using SaasFeeGuides.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface IAccountRepository
    {
        Task DeleteAccount(string userId);
    }
    public class AccountRepository : DataAccessBase, IAccountRepository
    {
        public AccountRepository(string connectionString) : base(connectionString)
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
                   
                    command.CommandText = "[Activities].[DeleteAccount]";

                    await command.ExecuteNonQueryAsync();
                    
                }
            }
        }

        

        
    }
}
