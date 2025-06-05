namespace AnyIssues.Controllers;

[ApiController]
[Route("api/bitbucket/issues")]
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
        
    [HttpPut("{issueNumber:int}")]
    public async Task<IActionResult> UpdateIssue(int issueNumber)
    {
        var body = await new StreamReader(Request.Body).ReadToEndAsync();
        var response = await bitbucketService.UpdateIssueAsync(issueNumber, body);
        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
        
    [HttpDelete("{issueNumber:int}")]
    public async Task<IActionResult> CloseIssue(int issueNumber)
    {
        var response = await bitbucketService.CloseIssueAsync(issueNumber);
        return response.StatusCode == System.Net.HttpStatusCode.OK
            ? Content(response.Json, "application/json")
            : Problem($"Bitbucket API returned status code {response.StatusCode}");
    }
}