using Amazon;
using SMARTAssessment.Api;
using SMARTAssessment.Api.Endpoints;
using SMARTAssessment.Application;
using SMARTAssessment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;

builder.Configuration.AddSecretsManager(
    region: RegionEndpoint.USEast1,
    configurator: options =>
    {
        options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
        options.KeyGenerator = (_, secretName) => secretName
            .Replace($"{env}_{appName}_", string.Empty)
            .Replace("__", ":");    
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddTransient<HttpRequestMiddleware>();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<HttpRequestMiddleware>();
app.MapLocationEndpoints();

app.Run();