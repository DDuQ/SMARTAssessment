using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class NavigationPointDto
{
    [JsonPropertyName("location")]
    public NavLocationDto Location { get; set; }

    [JsonPropertyName("restricted_travel_modes")]
    public List<string> RestrictedTravelModes { get; set; }
}