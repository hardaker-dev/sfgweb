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
    public interface ICustomerRepository
    {
        Task<int> UpsertCustomer(Customer customer);
        Task<(int customerBookingId, int activityDateId)> UpsertCustomerBooking(CustomerBooking booking);
        Task<Customer> SelectCustomerByUserId(string userId);
        Task DeleteAccount(string userId);
        Task<IEnumerable<Customer>> SelectCustomers(string emailSearch, string firstNameSearch, string lastNameSearch);
        Task<Customer> SelectCustomer(int id);
        Task<IEnumerable<CustomerBooking>> SelectCustomerBookings(int customerId);
        Task DeleteCustomerBooking(int activitySkuDateId, string customerEmail);
    }
    public class CustomerRepository : DataAccessBase, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }
        public async Task<(int customerBookingId, int activityDateId)> UpsertCustomerBooking(CustomerBooking booking)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PriceAgreed", booking.PriceAgreed);
                    command.Parameters.AddWithValue("@Email", booking.CustomerEmail);
                    command.Parameters.AddWithValue("@NumPersons", booking.NumPersons);
                    command.Parameters.AddWithValue("@HasPaid", booking.HasPaid);
                    command.Parameters.AddWithValue("@HasConfirmed", booking.HasConfirmed);
                    command.Parameters.AddWithValue("@CustomerNotes", booking.CustomerNotes ?? string.Empty);
                    if (booking.Id.HasValue)
                    {
                        command.Parameters.AddWithValue("@Id", booking.Id.Value);
                     
                        command.Parameters.AddWithValue("@HasCancelled", booking.HasCancelled);
                        command.CommandText = "[Activities].[UpdateCustomerBooking]";
                        await command.ExecuteNonQueryAsync();
                        return (booking.Id.Value, booking.ActivityDateSkuId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ActivitySkuName", booking.ActivitySkuName);
                        command.Parameters.AddWithValue("@Date", booking.DateTime);
                        command.CommandText = "[Activities].[InsertCustomerBooking]";
                        return await ReadSingleValueAsync(command, (reader) =>

                         {
                             var customerBookingId = GetInt(reader, 0).Value;
                             var activityDateId = GetInt(reader, 1).Value;

                             return (customerBookingId, activityDateId);
                         });
                    }                    
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

        public async Task<IEnumerable<Customer>> SelectCustomers(string emailSearch,string firstNameSearch,string lastNameSearch)
        {
            using (var cn = await GetNewConnectionAsync())
            {
                using (var command = cn.CreateCommand())
                {
                    
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@emailSearch", (object)emailSearch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@firstNameSearch", (object)firstNameSearch ?? DBNull.Value);
                    command.Parameters.AddWithValue("@lastNameSearch", (object)lastNameSearch ?? DBNull.Value);
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

        public Task<IEnumerable<CustomerBooking>> SelectCustomerBookings(int customerId)
        {
            throw new NotImplementedException();
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

       
    }
}
