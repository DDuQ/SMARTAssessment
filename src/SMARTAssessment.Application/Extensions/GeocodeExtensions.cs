using System.Text.RegularExpressions;
using SMARTAssessment.Domain.Entities;
using SMARTAssessment.Infrastructure.Clients.Google.Dtos;

namespace SMARTAssessment.Application.Extensions;

public static class GeocodeExtensions
{
    /// <summary>
    /// Transforms an address string into a partition key by removing leading
    /// and trailing whitespace and converting it to lowercase.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static string ToKey(this string address) 
    {
        if (string.IsNullOrWhiteSpace(address))
            return string.Empty;
            
        return Regex.Replace(
            address.Trim().ToLowerInvariant(),
            "[^a-z0-9 ]+", // Keep only alphanumeric and spaces
            string.Empty)
            .Replace(" ", "-") // Replace spaces with hyphens
            .Replace("--", "-") // Remove any double hyphens
            .Trim('-'); // Remove any leading/trailing hyphens
    }

    /// <summary>
    /// Converts a GeocodeDto from the Google Geocoding API to a Geocode domain entity.
    /// </summary>
    /// <param name="geocodeDto">The DTO to convert</param>
    /// <returns>A Geocode entity with all nested objects mapped</returns>
    public static Geocode ToEntity(this GeocodeDto geocodeDto)
    {
        return new Geocode
        {
            Status = geocodeDto.Status,
            Results = geocodeDto.Results?.Select(ToResult).ToList()
        };
    }

    public static Geocode ToEntity(this GeocodeDto geocodeDto, string userAddress)
    {
        var key = userAddress.ToKey();
        return new Geocode
        {
            Pk = key,
            Sk = key,
            Status = geocodeDto.Status,
            Results = geocodeDto.Results?.Select(ToResult).ToList()
        };
    }
    private static Result ToResult(ResultDto resultDto)
    {
        return new Result
        {
            FormattedAddress = resultDto.FormattedAddress,
            PlaceId = resultDto.PlaceId,
            Types = resultDto.Types,
            AddressComponents = resultDto.AddressComponents?.Select(ToAddressComponent).ToList(),
            Geometry = ToGeometry(resultDto.GeometryDto),
            NavigationPoints = resultDto.NavigationPoints?.Select(ToNavigationPoint).ToList()
        };
    }

    private static AddressComponent ToAddressComponent(AddressComponentDto dto)
    {
        return new AddressComponent
        {
            LongName = dto.LongName,
            ShortName = dto.ShortName,
            Types = dto.Types
        };
    }

    private static Geometry? ToGeometry(GeometryDto? geometryDto)
    {
        if (geometryDto == null) return null;

        return new Geometry
        {
            LocationType = geometryDto.LocationType,
            Location = ToLocation(geometryDto.LocationDto),
            Viewport = ToViewport(geometryDto.ViewportDto),
            Bounds = ToBounds(geometryDto.BoundsDto)
        };
    }

    private static Location? ToLocation(LocationDto? locationDto)
    {
        if (locationDto == null) return null;

        return new Location
        {
            Lat = locationDto.Lat,
            Lng = locationDto.Lng
        };
    }

    private static Viewport? ToViewport(ViewportDto? viewportDto)
    {
        if (viewportDto == null) return null;

        return new Viewport
        {
            Northeast = ToLocation(viewportDto.Northeast),
            Southwest = ToLocation(viewportDto.Southwest)
        };
    }

    private static Bounds? ToBounds(BoundsDto? boundsDto)
    {
        if (boundsDto == null) return null;

        return new Bounds
        {
            Northeast = ToLocation(boundsDto.Northeast),
            Southwest = ToLocation(boundsDto.Southwest)
        };
    }

    private static NavigationPoint ToNavigationPoint(NavigationPointDto navigationPointDto)
    {
        return new NavigationPoint
        {
            RestrictedTravelModes = navigationPointDto.RestrictedTravelModes,
            Location = ToNavLocation(navigationPointDto.Location)
        };
    }

    private static NavLocation? ToNavLocation(NavLocationDto? navLocationDto)
    {
        if (navLocationDto == null) return null;

        return new NavLocation
        {
            Latitude = navLocationDto.Latitude,
            Longitude = navLocationDto.Longitude
        };
    }
}