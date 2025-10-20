using SMARTAssessment.Application.Extensions;
using SMARTAssessment.Application.Interfaces;
using SMARTAssessment.Domain.Entities;

namespace SMARTAssessment.Application.Services;

public class GeocodeService : IGeocodeService
{
    private readonly IGeocodeRepository _geocodeRepository;
    private readonly IGeocodeClient _geocodeClient;
    
    public GeocodeService(IGeocodeRepository geocodeRepository, IGeocodeClient geocodeClient)
    {
        _geocodeRepository = geocodeRepository;
        _geocodeClient = geocodeClient;
    }
    
    public async Task<Geocode> GetLocationByAddress(string address)
    {
        var cachedGeocode = await _geocodeRepository.GetGeocodeByAddress(address);
        
        if (cachedGeocode != null)
        {
            return cachedGeocode;
        }
        
        var encodedAddress = Uri.EscapeDataString(address);
        var geocodeDto = await _geocodeClient.GetGeocodeByAddress(encodedAddress);
        await _geocodeRepository.CreateGeocode(geocodeDto.ToEntity(address));
        return geocodeDto.ToEntity();
    }
}