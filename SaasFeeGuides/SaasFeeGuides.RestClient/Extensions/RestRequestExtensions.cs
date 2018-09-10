using RiskFirst.RestClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasFeeGuides.RestClient.Extensions
{
    internal static class RestRequestExtensions
    {
        internal static RestRequest AppendAdditionalHeaders(this RestRequest restRequest, Dictionary<string, object> additionalHeaders)
        {
            if (additionalHeaders != null && additionalHeaders.Count > 0)
            {
                return restRequest.WithHeaders(additionalHeaders);
            }
            return restRequest;
        }

        internal static RestRequest AcceptGzipCompression(this RestRequest restRequest)
        {
            return restRequest.WithHeader("Accept-Encoding", "gzip");
        }
    }
}
