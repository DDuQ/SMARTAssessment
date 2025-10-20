using System.Text.Json.Serialization;

namespace SMARTAssessment.Infrastructure.Clients.Google.Dtos;

public class GeocodeDto
{
    [JsonPropertyName("results")]
    public List<ResultDto> Results { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}