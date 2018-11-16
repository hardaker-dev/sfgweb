using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Extensions
{
    public static class ModelReaderExtensions
    {
        public static int? GetInteger(this SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<int>(fieldNum);
        }

        public static double? GetDub(this SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<double>(fieldNum);
        }

        public static bool? GetBool(this SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<bool>(fieldNum);
        }

        public static string GetStr(this SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<string>(fieldNum);
        }
        public static DateTime? GetDt(this SqlDataReader reader, int fieldNum)
        {
            if (fieldNum >= reader.FieldCount)
            {
                throw new Exception($"fieldNum of {fieldNum} is larger than the number of fields {reader.FieldCount}");
            }
            if (reader.IsDBNull(fieldNum))
            {
                return null;
            }
            return reader.GetFieldValue<DateTime>(fieldNum);
        }

        public static Models.CustomerBooking ReadCustomerBooking(this SqlDataReader reader)
        {
            try
            {
                return new Models.CustomerBooking
                {
                    Id = reader.GetInteger(0).Value,
                    ActivitySkuName =  reader.GetStr( 1),
                    ActivityDateSkuId = reader.GetInteger(3).Value,
                    HasConfirmed = reader.GetBool(4) ?? false,
                    HasPaid = reader.GetBool(5) ?? false,
                    CustomerEmail = reader.GetStr(6),
                    NumPersons = reader.GetInteger(7).Value,
                    PriceAgreed = reader.GetDub(8) ?? 0,
                    DateTime = reader.GetDt(9).Value,
                    CustomerDisplayName = reader.GetStr(10),
                    CustomerNotes = reader.GetStr(11)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Customer Booking", ex);
            }
        }
    }
}
