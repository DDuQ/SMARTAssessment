using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using SMARTAssesment.Application.Interfaces;
using SMARTAssesment.Infrastructure.Clients.Google.Dtos;

namespace SMARTAssesment.Infrastructure.Clients.Google;

public class GeocodingClient(HttpClient httpClient, IConfiguration configuration) : IGeocodeClient
{
    private readonly string _apiKey = configuration["GeocodingApiKey"]!;
    private const string Json = "json";
    
    public async Task<GeocodeDto> GetLocationByAddress(string address)
    {
        var query = $"{Json}?address={address}&key={_apiKey}";
        return await ExecuteQuery(query);
    }

    public async Task<GeocodeDto> GetLocationByCoordinates(CoordinatesDto coordinatesDto)
    {
        var query = $"{Json}?latlng={coordinatesDto.Latitude},{coordinatesDto.Longitude}&key={_apiKey}";
        return await ExecuteQuery(query);
    }
    private async Task<GeocodeDto> ExecuteQuery(string query)
    {
        var geocodingResponse = await httpClient.GetAsync(query);
        var json = await geocodingResponse.Content.ReadAsStringAsync();
        var addressResult = await geocodingResponse.Content.ReadFromJsonAsync<GeocodeDto>();
        
        return HandleGeocodeResponse(addressResult);
    }

    private static GeocodeDto HandleGeocodeResponse(GeocodeDto? addressResult)
    {
        return addressResult?.Status switch
        {
            "OK" => addressResult,
            "ZERO_RESULTS" => 
                throw new HttpRequestException("Could not find the location", null, HttpStatusCode.UnprocessableContent),
            "OVER_QUERY_LIMIT" or "OVER_DAILY_LIMIT" or "REQUEST_DENIED" or "UNKNOWN_ERROR" =>
                throw new HttpRequestException("Geocoding provider failed", null, HttpStatusCode.BadGateway),
            "INVALID_REQUEST" =>
                throw new HttpRequestException("Invalid input", null, HttpStatusCode.BadRequest),
            _ => 
                throw new HttpRequestException("Unexpected status received from geocoding API",
                    null,
                    HttpStatusCode.InternalServerError)
        };
    }
}