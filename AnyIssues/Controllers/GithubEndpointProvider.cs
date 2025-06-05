namespace AnyIssues.Controllers;

public static class GithubEndpointProvider
{
    public static void AddGithubEndpoints(this IEndpointRouteBuilder app)
    {
        var githubGroup = app.MapGroup("api/github/issues");

        githubGroup.MapGet("/{issueNumber:int}", async (int issueNumber, GithubService github) =>
            await github.GetIssueAsync(issueNumber)
        ).WithName("GetGitHubIssue");
    
        githubGroup.MapPost("", async (GithubService github, HttpRequest request) =>
            await github.CreateIssueAsync(request)
        ).WithName("CreateGitHubIssue");
    
        githubGroup.MapPut("/{issueNumber:int}", async (int issueNumber, GithubService github, HttpRequest request) =>
            await github.UpdateIssueAsync(issueNumber, request)
        ).WithName("UpdateGitHubIssue");
    
        githubGroup.MapDelete("/{issueNumber:int}", async (int issueNumber, GithubService github, HttpRequest request) =>
            await github.CloseIssueAsync(issueNumber, request)
        ).WithName("CloseGitHubIssue");
    }
}