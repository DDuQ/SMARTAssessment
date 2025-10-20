using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class LocationDto
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}