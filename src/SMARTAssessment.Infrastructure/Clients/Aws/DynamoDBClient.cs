using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using SMARTAssessment.Application.Extensions;
using SMARTAssessment.Application.Interfaces;
using SMARTAssessment.Domain.Entities;

namespace SMARTAssessment.Infrastructure.Clients.Aws;

public class GeocodeRepository : IGeocodeRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string? _tableName; 

    public GeocodeRepository(IAmazonDynamoDB dynamoDb, IConfiguration configuration)
    {
        _dynamoDb = dynamoDb;
        _tableName = configuration["DynamoDB:table"];
    }

    public async Task<Geocode?> GetGeocodeByAddress(string address)
    {
        var addressAsKey = address.ToKey();
        
        var request = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "pk", new AttributeValue { S = addressAsKey } },
                { "sk", new AttributeValue { S = addressAsKey } }
            }
        };

        var response = await _dynamoDb.GetItemAsync(request);

        if (response.Item is null)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<Geocode>(itemAsDocument.ToJson());
    }

    public async Task<bool> CreateGeocode(Geocode geocode)
    {
        var item = JsonSerializer.Serialize(geocode);
        var itemAttributes = Document.FromJson(item).ToAttributeMap();
        
        var request = new PutItemRequest()
        {
            TableName = _tableName,
            Item = itemAttributes,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exists(sk)"
        };

        try
        {
            var response = await _dynamoDb.PutItemAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (ConditionalCheckFailedException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

