using SMARTAssessment.Infrastructure.Clients.Google.Dtos;

namespace SMARTAssessment.Application.Interfaces;

public interface IGeocodeClient
{
    Task<GeocodeDto> GetGeocodeByAddress(string address);
}