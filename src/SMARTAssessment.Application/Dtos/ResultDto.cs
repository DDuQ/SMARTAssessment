using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class ResultDto
{
    [JsonPropertyName("address_components")]
    public List<AddressComponentDto> AddressComponents { get; set; }

    [JsonPropertyName("formatted_address")]
    public string FormattedAddress { get; set; }

    [JsonPropertyName("geometry")]
    public GeometryDto GeometryDto { get; set; }

    [JsonPropertyName("navigation_points")]
    public List<NavigationPointDto> NavigationPoints { get; set; }

    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; }

    [JsonPropertyName("types")]
    public List<string> Types { get; set; }
}