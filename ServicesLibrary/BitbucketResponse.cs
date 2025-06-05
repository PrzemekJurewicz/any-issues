namespace ServicesLibrary;

public readonly struct BitbucketResponse(string json, HttpStatusCode statusCode)
{
    public string Json { get; } = json;
    public HttpStatusCode StatusCode { get; } = statusCode;
}