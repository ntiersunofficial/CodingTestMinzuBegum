using CountryAPP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CountryAPP.Core.Contract.Persistence;

namespace CountryAPP.API.Persistence;

public class CountryRepository : ICountryRepository
{
    private readonly HttpClient _httpClient;

    public CountryRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CountryResponseModel>> GetCountriesAsync()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync("https://restcountries.com/v3.1/all?fields=name,flags");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();

        var countries = JsonSerializer.Deserialize<List<RawCountryModel>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Map and sort by CommonName (ascending)
        return countries
            .Select(c => new CountryResponseModel
            {
                Name = c.Name?.Common,
                Flag = c.Flags?.Png
            })
            .Where(c => !string.IsNullOrEmpty(c.Name)) // Optional: ignore nulls
            .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<CountryDetailResponseModel> GetCountryDetailsAsync(string countryName)
    {
        using var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var client = new HttpClient(handler);
        var response = await client.GetAsync($"https://restcountries.com/v3.1/name/{countryName}");

        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();

        var countries = JsonSerializer.Deserialize<List<RawCountryModel>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var country = countries?.FirstOrDefault();
        if (country == null) return null;

        return new CountryDetailResponseModel
        {
            Name = country.Name?.Common,
            Area = country.Area,
            Population = country.Population,
            Timezone = country.Timezones?.FirstOrDefault(),
            Tld = country.Tld?.FirstOrDefault(),
            Capital = country.Capital?.FirstOrDefault(),
            Latitude = country.Latlng?.ElementAtOrDefault(0),
            Longitude = country.Latlng?.ElementAtOrDefault(1),
            DrivingSide = country.Car?.Side,
            Continent = country.Continents?.FirstOrDefault()
        };
    }
}
