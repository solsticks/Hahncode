using Hahn.ApplicationProcess.December2020.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain.Services
{
    public static class HttpClientService
    {

        public static async Task<Response<string>> ValidateCountryName(string countryName)
        {
            var response = new Response<string>();
            using var client = new HttpClient();

            var url = $"https://restcountries.eu/rest/v2/name/{countryName}?fullText=true";

            var jsonResponse = await client.GetAsync(url);
            client.Dispose();

            if (!jsonResponse.IsSuccessStatusCode)
            {
                response.Message = $"{countryName} is not a valid country name";
                response.Success = false;
                return response;
            }
            var json = await jsonResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Country>>(json);

            response.Success = true;
            response.Data = result.Select(x => x.Name).ToString();

            return response;
        }
    }
}
