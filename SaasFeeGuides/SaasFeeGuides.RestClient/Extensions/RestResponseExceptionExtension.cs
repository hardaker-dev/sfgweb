using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RiskFirst.RestClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.RestClient.Extensions
{
    public static class RestResponseExceptionExtension
    {
        public static dynamic GetJsonBody(this RestResponseException restResponseException)
        {
            return JsonConvert.DeserializeObject(restResponseException.Body);
        }
    }
}
