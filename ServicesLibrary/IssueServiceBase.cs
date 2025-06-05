namespace ServicesLibrary;

public abstract class IssueServiceBase(IHttpClientFactory httpClientFactory)
{
    protected virtual HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("AnyIssuesApp");
        
        return client;
    }
}
