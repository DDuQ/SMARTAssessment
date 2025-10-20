using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class GeometryDto
{
    [JsonPropertyName("location")]
    public LocationDto LocationDto { get; set; }

    [JsonPropertyName("location_type")]
    public string LocationType { get; set; }

    [JsonPropertyName("viewport")]
    public ViewportDto ViewportDto { get; set; }

    [JsonPropertyName("bounds")]
    public BoundsDto BoundsDto { get; set; }
}