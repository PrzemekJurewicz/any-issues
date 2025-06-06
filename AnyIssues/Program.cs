var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddScoped<GithubService>();
builder.Services.AddScoped<BitbucketService>();

// Add API versioning
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new QueryStringApiVersionReader("api-version"),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddMvc()
    .EnableApiVersionBinding()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

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