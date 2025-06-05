namespace ServicesLibrary;

public class GithubService(IHttpClientFactory httpClientFactory, IConfiguration config) : IssueServiceBase(httpClientFactory)
{
    private readonly string _owner =
        config["GITHUB:OWNER"] ?? throw new InvalidOperationException("GITHUB:OWNER not set");

    private readonly string _repo =
        config["GITHUB:REPO"] ?? throw new InvalidOperationException("GITHUB:REPO not set");

    private readonly string _token =
        config["GITHUB:TOKEN"] ?? throw new InvalidOperationException("GITHUB:TOKEN not set");

    protected override HttpClient CreateClient()
    {
        var client = base.CreateClient();
        
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        return client;
    }

    public async Task<IResult> GetIssueAsync(int issueNumber)
    {
        var client = CreateClient();
        var url = $"https://api.github.com/repos/{_owner}/{_repo}/issues/{issueNumber}";
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"GitHub API returned status code {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();
        return Results.Content(json, "application/json");
    }

    public async Task<IResult> CreateIssueAsync(HttpRequest request)
    {
        var client = CreateClient();
        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        var url = $"https://api.github.com/repos/{_owner}/{_repo}/issues";
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"GitHub API returned status code {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();
        return Results.Content(json, "application/json");
    }

    public async Task<IResult> UpdateIssueAsync(int issueNumber, HttpRequest request)
    {
        var client = CreateClient();
        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync();
        var url = $"https://api.github.com/repos/{_owner}/{_repo}/issues/{issueNumber}";
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        // GitHub API requires a PATCH request to update an issue
        var response = await client.PatchAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"GitHub API returned status code {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();
        return Results.Content(json, "application/json");
    }

    public async Task<IResult> CloseIssueAsync(int issueNumber, HttpRequest request)
    {
        var client = CreateClient();
        var url = $"https://api.github.com/repos/{_owner}/{_repo}/issues/{issueNumber}";
        
        // GitHub API expects a JSON body to close an issue
        var body = "{ \"state\": \"closed\" }";
        var content = new StringContent( body,System.Text.Encoding.UTF8, "application/json");

        // GitHub API requires a PATCH request to close an issue
        var response = await client.PatchAsync(url, content);

        return !response.IsSuccessStatusCode
            ? Results.Problem($"GitHub API returned status code {response.StatusCode}")
            : Results.Ok($"Issue {issueNumber} closed successfully.");
    }
}