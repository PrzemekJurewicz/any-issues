namespace ServicesLibrary;

public class BitbucketService(IHttpClientFactory httpClientFactory, IConfiguration config) : IssueServiceBase(httpClientFactory)
{
    private readonly string _workspace = 
        config["BITBUCKET:WORKSPACE"] ?? throw new InvalidOperationException("BITBUCKET:WORKSPACE not set");

    private readonly string _repoSlug =
        config["BITBUCKET:REPO"] ?? throw new InvalidOperationException("BITBUCKET:REPO not set");

    private readonly string _username =
        config["BITBUCKET:USERNAME"] ?? throw new InvalidOperationException("BITBUCKET:USERNAME not set");

    private readonly string _appPassword =
        config["BITBUCKET:APP_PASSWORD"] ?? throw new InvalidOperationException("BITBUCKET:APP_PASSWORD not set");
    
    protected override HttpClient CreateClient()
    {
        var client = base.CreateClient();

        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_username}:{_appPassword}");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        return client;
    }

    public async Task<BitbucketResponse> ListIssuesAsync()
    {
        var client = CreateClient();
        var url = $"https://api.bitbucket.org/2.0/repositories/{_workspace}/{_repoSlug}/issues";
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        return new BitbucketResponse(json, response.StatusCode);
    }

    public async Task<BitbucketResponse> CreateIssueAsync(string body)
    {
        var client = CreateClient();
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        var url = $"https://api.bitbucket.org/2.0/repositories/{_workspace}/{_repoSlug}/issues";
        var response = await client.PostAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();
        return new BitbucketResponse(json, response.StatusCode);
    }

    public async Task<BitbucketResponse> UpdateIssueAsync(int issueNumber, string body)
    {
        var client = CreateClient();
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        var url = $"https://api.bitbucket.org/2.0/repositories/{_workspace}/{_repoSlug}/issues/{issueNumber}";
        var response = await client.PutAsync(url, content);
        var json = await response.Content.ReadAsStringAsync();
        return new BitbucketResponse(json, response.StatusCode);
    }

    public async Task<BitbucketResponse> CloseIssueAsync(int issueNumber)
    {
        var client = CreateClient();
        var url = $"https://api.bitbucket.org/2.0/repositories/{_workspace}/{_repoSlug}/issues/{issueNumber}";
        var content = new StringContent("{\"state\": \"closed\"}", System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync(url, content); // Bitbucket API requires a PUT to close an issue
        var json = await response.Content.ReadAsStringAsync();
        return new BitbucketResponse(json, response.StatusCode);
    }
}