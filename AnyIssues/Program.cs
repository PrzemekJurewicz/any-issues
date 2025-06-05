var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddScoped<GithubService>();
builder.Services.AddScoped<BitbucketService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// GitHub MinimalAPI Endpoints. For multiple providers use Carter.
app.AddGithubEndpoints();

// Bitbucket API Endpoints
app.MapControllers();

app.Run();