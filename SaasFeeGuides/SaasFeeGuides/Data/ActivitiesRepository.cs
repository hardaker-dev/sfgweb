using SaasFeeGuides.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface IActivitiesRepository
    {
        Task<int> UpsertCustomer(Customer customer);
        Task<Customer> SelectCustomerByUserId(string userId);
    }
    public class ActivitiesRepository : DataAccessBase, IActivitiesRepository
    {
        public ActivitiesRepository(string connectionString) : base(connectionString)
        {
        }


        public async Task<Customer> SelectCustomerByUserId(string userId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    
                    command.CommandType = CommandType.StoredProcedure;
                   
                    command.CommandText = "[Activities].[SelectCustomerByUserId]";

                    return await ReadSingleAsync(command,ReadCustomer);
                    
                }
            }
        }

        private Customer ReadCustomer(SqlDataReader reader)
        {
            try
            {
                return new Customer
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

        public async Task<int> UpsertCustomer(Customer customer)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth == DateTime.MinValue ? DBNull.Value : (object)customer.DateOfBirth);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@UserId", customer.UserId);
                    command.Parameters.AddWithValue("@Address", customer.Address);
                    command.CommandType = CommandType.StoredProcedure;
                    if (customer.Id.HasValue)
                    {
                        command.CommandText = "[Activities].[UpdateCustomer]";                      

                        command.Parameters.AddWithValue("@Id", customer.Id.Value);
                        
                        await command.ExecuteNonQueryAsync();
                        return customer.Id.Value;
                    }
                    else
                    {
                        command.CommandText = "[Activities].[InsertCustomer]";
                        var result = await command.ExecuteScalarAsync();
                        return (int)result;
                    }
                }
            }
        }
    }
}
