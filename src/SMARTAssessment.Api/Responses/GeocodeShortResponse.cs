namespace SMARTAssessment.Api.Contracts.Responses;

public class GeocodeShortResponse
{
    public CoordinatesResponse Coordinates { get; set; }
    public string Address { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CoordinatesResponse
{
    public double Latitude { get; set; } 
    public double Longitude { get; set; }
}