using SMARTAssessment.Domain.Entities;

namespace SMARTAssessment.Application.Interfaces;

public interface IGeocodeService
{
    Task<Geocode> GetLocationByAddress(string address);
}