using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using System;
using System.IO;
using System.Net.Http;
using Thesis_BAP;

// Configure app
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPredictionEnginePool<BAP_WMS_MODEL.ModelInput, BAP_WMS_MODEL.ModelOutput>()
    .FromFile("BAP_WMS_MODEL.mlnet");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Description = "Docs for my API", Version = "v1" });
});

builder.Services.AddCors(); // Add CORS service

// Add MongoDB client
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var connectionString = "mongodb+srv://aaronvanmarcke:7njLFZi8tE42jOoD@mlnet.5mufwtx.mongodb.net/?retryWrites=true&w=majority&appName=MLNET";
    return new MongoClient(connectionString);
});

var app = builder.Build();

// Configure CORS
app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

var collectionName = "WMS";

app.MapPost("/predict", async (HttpContext httpContext, PredictionEnginePool<BAP_WMS_MODEL.ModelInput, BAP_WMS_MODEL.ModelOutput> predictionEnginePool) =>
{
    using var reader = new StreamReader(httpContext.Request.Body);
    var requestBody = await reader.ReadToEndAsync();
    Console.WriteLine("Request Body: " + requestBody);

    // Deserialize request body into ModelInput
    var input = JsonSerializer.Deserialize<BAP_WMS_MODEL.ModelInput>(requestBody);
    if (input == null)
    {
        httpContext.Response.StatusCode = 400; // Bad Request
        await httpContext.Response.WriteAsync("Invalid request data.");
        return;
    }

    // Make prediction
    var prediction = predictionEnginePool.Predict(input);
    Console.WriteLine("Prediction: " + JsonSerializer.Serialize(prediction));

    // Return prediction as JSON response
    httpContext.Response.ContentType = "application/json";
    await JsonSerializer.SerializeAsync(httpContext.Response.Body, prediction);
});


// Run app
app.Run();
