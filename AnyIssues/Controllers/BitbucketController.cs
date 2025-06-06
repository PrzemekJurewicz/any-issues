namespace AnyIssues.Controllers;
    
[ApiVersion(1)]
[ApiController]
[Route("api/[controller]/issues")]
[Route("api/v{v:apiVersion}/[controller]/issues")]
public class BitbucketController(BitbucketService bitbucketService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> ListIssues()
    {
        var response = await bitbucketService.ListIssuesAsync();
        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateIssue()
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        var response = await bitbucketService.CreateIssueAsync(body);
        return response.StatusCode == System.Net.HttpStatusCode.Created
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
    
    [ApiVersion("1.0")]
    [HttpPut("{issueNumber:int}")]
    public async Task<IActionResult> UpdateIssue(int issueNumber)
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        var response = await bitbucketService.UpdateIssueAsync(issueNumber, body);
        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
    
    [ApiVersion("1.0")]
    [HttpDelete("{issueNumber:int}")]
    public async Task<IActionResult> CloseIssue(int issueNumber)
    {
        var response = await bitbucketService.CloseIssueAsync(issueNumber);
        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
}