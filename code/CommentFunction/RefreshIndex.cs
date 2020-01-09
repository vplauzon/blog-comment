using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommentFunction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Octokit;

namespace SearchFunction
{
    public static class RefreshIndex
    {
        [FunctionName("submit-comment")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequest request,
            ILogger log)
        {
            const string REPO_OWNER = "vplauzon";
            const string REPO_NAME = "vplauzon.github.io";

            log.LogInformation($"Comment at: {DateTime.Now}");
            try
            {
                var commentRequest = await RetrieveCommentAsync(request, log);
                var comment = new Comment(commentRequest);
                var client = GetGitHubClient();
                var repo = await client.Repository.Get(REPO_OWNER, REPO_NAME);
                var defaultBranch =
                    await client.Repository.Branch.Get(repo.Id, repo.DefaultBranch);
                //  Create new branch
                var newBranch = await client.Git.Reference.Create(
                    repo.Id,
                    new NewReference(
                        $"refs/heads/comment-{comment.Id}",
                        defaultBranch.Commit.Sha));
                var yamlComment = comment.AsYaml();
                // Create a new file with the comments in it
                var fileRequest = new CreateFileRequest(
                    $"Comment submitted through Azure Function",
                    yamlComment,
                    newBranch.Ref)
                {
                    Committer = new Committer("commenter", "commenter@commenter.com", DateTime.Now)
                };

                log.LogInformation("Comment:");
                log.LogInformation(yamlComment);
                await client.Repository.Content.CreateFile(
                    repo.Id,
                    $"_data/comments/{commentRequest.PagePath}/{comment.Id}.yaml",
                    fileRequest);

                // Create a pull request for the new branch and file
                var pr = await client.Repository.PullRequest.Create(
                    repo.Id,
                    new NewPullRequest(fileRequest.Message, newBranch.Ref, defaultBranch.Name)
                    {
                        Body = $"From {comment.Author.Name} on {commentRequest.PagePath}"
                    });


                return new OkObjectResult("Comment Submitted");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception:  {ex.Message}");
                log.LogError($"Inner Exception:  {ex.InnerException?.Message}");
                log.LogError($"Exception type:  {ex.GetType().FullName}");

                throw;
            }
        }

        private static GitHubClient GetGitHubClient()
        {
            var githubUser = Environment.GetEnvironmentVariable("githubUserName");
            var githubPassword = Environment.GetEnvironmentVariable("githubPassword");
            var basicAuth = new Credentials(githubUser, githubPassword);
            var client = new GitHubClient(new ProductHeaderValue("user-comment"));

            client.Credentials = basicAuth;

            return client;
        }

        private static async Task<CommentRequest> RetrieveCommentAsync(
            HttpRequest request,
            ILogger log)
        {
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            var commentRequest = JsonSerializer.Deserialize<CommentRequest>(requestBody);

            log.LogInformation($"Request:  {requestBody}");

            return commentRequest;
        }
    }
}