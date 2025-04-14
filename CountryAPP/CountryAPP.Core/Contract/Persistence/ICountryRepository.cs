using CountryAPP.Core.Model;

namespace CountryAPP.Core.Contract.Persistence;
public interface ICountryRepository
{
    Task<List<CountryResponseModel>> GetCountriesAsync();

    Task<CountryDetailResponseModel> GetCountryDetailsAsync(string countryName);
}

