using Microsoft.AspNetCore.Http;
using Service.Helpers;
using System.Text.Json;

namespace Service.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Permet l'ajout d'un paramètre de pagination dans le header de la requète
        /// </summary>
        /// <param name="response"></param>
        /// <param name="currentPage">Qu'elle page on souhaite afficher</param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalItems"></param>
        /// <param name="totalPages"></param>
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            PaginationHeader paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Création des champs dans la requête
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
