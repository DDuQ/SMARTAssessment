using SMARTAssessment.Domain.Entities;

namespace SMARTAssessment.Application.Interfaces;

public interface IGeocodeRepository
{
    Task<Geocode?> GetGeocodeByAddress(string address);
    Task<bool> CreateGeocode(Geocode geocode);
}