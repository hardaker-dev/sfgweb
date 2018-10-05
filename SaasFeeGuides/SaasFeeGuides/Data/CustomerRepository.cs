using SaasFeeGuides.Exceptions;
using SaasFeeGuides.Helpers;
using SaasFeeGuides.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Data
{
    public interface ICustomerRepository
    {
        Task<int> UpsertCustomer(Customer customer);
        Task<int> InsertCustomerBooking(CustomerBooking booking);
        Task<Customer> SelectCustomerByUserId(string userId);
        Task DeleteAccount(string userId);
        Task<IEnumerable<Customer>> SelectCustomers();
        Task<Customer> SelectCustomer(int id);
        Task<IEnumerable<CustomerBooking>> SelectCustomerBookings(int customerId);
    }
    public class CustomerRepository : DataAccessBase, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task<int> InsertCustomerBooking(CustomerBooking booking)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@ActivitySkuName", booking.ActivitySkuName);
                    command.Parameters.AddWithValue("@PriceAgreed", booking.PriceAgreed);
                    command.Parameters.AddWithValue("@Email", booking.CustomerEmail);
                    command.Parameters.AddWithValue("@Date", booking.DateTime);
                    command.Parameters.AddWithValue("@NumPersons", booking.NumPersons);
                    command.Parameters.AddWithValue("@HasPaid", booking.HasPaid);
                    command.Parameters.AddWithValue("@HasConfirmed", booking.HasConfirmed);
                    command.CommandType = CommandType.StoredProcedure;
                   
                    command.CommandText = "[Activities].[InsertCustomerBooking]";
                    var result = await command.ExecuteScalarAsync();
                    return (int)result;
                    
                }
            }
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

        public async Task<Customer> SelectCustomer(int customerId)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@customerId", customerId);
                    command.CommandText = "[Activities].[SelectCustomer]";

                    return await ReadSingleAsync(command, ReadCustomer);

                }
            }
        }

        public async Task<IEnumerable<Customer>> SelectCustomers()
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "[Activities].[SelectCustomers]";

                    return await ReadListAsync(command, ReadCustomer);

                }
            }
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

                    return await ReadSingleAsync(command, ReadCustomer);

                }
            }
        }


        public async Task<int> UpsertCustomer(Customer customer)
        {
            try
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
            catch(SqlException e)
            {
                switch ((DataError)e.Number)
                {
                    case DataError.UniqueIndexViolation:
                        throw new BadRequestException($"Customer already exists with Email '{customer.Email}'",System.Net.HttpStatusCode.BadRequest, e);
                    default: throw e;
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

        public Task<IEnumerable<CustomerBooking>> SelectCustomerBookings(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
