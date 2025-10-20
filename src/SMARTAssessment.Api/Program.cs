using SMARTAssessment.Api;
using SMARTAssessment.Api.Endpoints;
using SMARTAssessment.Application;
using SMARTAssessment.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// builder.Configuration.AddSecretsManager(configurator: options =>
// {
//     options.SecretFilter = secret => secret.Name.StartsWith("SMARTAssessment/");
//     options.PollingInterval = TimeSpan.FromHours(1);
// });

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