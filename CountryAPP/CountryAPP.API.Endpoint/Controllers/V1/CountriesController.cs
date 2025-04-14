using CountryAPP.Core.Contract.Persistence;
using CountryAPP.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Emit;

namespace CountryAPP.API.Endpoint.Controllers.V1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;

    public CountriesController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet("GetCountry")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CountryResponseModel>>> GetCountryAsync()
    {
        var countries = await _countryRepository.GetCountriesAsync();
        return Ok(countries);
    }

    [HttpGet("GetCountryDetails/{countryName}"), AllowAnonymous]
    public async Task<ActionResult<CountryDetailResponseModel>> GetCountryDetailsAsync(string countryName)
    {
        var result = await _countryRepository.GetCountryDetailsAsync(countryName);
        if (result == null) return NotFound("Country not found");

        return Ok(result);
    }
}
