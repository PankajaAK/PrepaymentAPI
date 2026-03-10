using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.SemanticKernel;
using PrepaymentAPI.Infrastructure;
using PrepaymentAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Key Vault connection
var keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddApplicationInsightsTelemetry();

// Semantic Kernel
builder.Services.AddKernel();

// HTTP client
builder.Services.AddHttpClient();

// Register our services
builder.Services.AddSingleton<IAuditService,
    InMemoryAuditService>();
builder.Services.AddScoped<IDocumentIngestionService,
    StubDocumentIngestionService>();
builder.Services.AddScoped<IRiskAnalysisService,
    AzureOpenAIService>();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prepayment Risk API v1");
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
