using Microsoft.OpenApi.Models;

namespace API
{
    internal class ApiKeyScheme : OpenApiSecurityScheme
    {
        public string In { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}