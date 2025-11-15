using SmartCRM.API.Models;
using System.Text.Json;

namespace SmartCRM.API.Extensions
{
    public static class HttpResponseExtentions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationMetadata meta)
        {
            response.Headers ["X-Pagination"] = JsonSerializer.Serialize(meta);

            response.Headers["Access-Control-Expose-Headers"] = "X-Pagination";
        }
    }
}
