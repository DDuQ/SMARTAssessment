using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class ViewportDto
{
    [JsonPropertyName("northeast")]
    public LocationDto Northeast { get; set; }

    [JsonPropertyName("southwest")]
    public LocationDto Southwest { get; set; }
}