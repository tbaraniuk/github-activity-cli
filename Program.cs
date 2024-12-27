using System;
using System.CommandLine;
using github_activity.src;

namespace GithubActivity
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var usernameArgument = new Argument<string>(name:"username", description:"A username for a user of GitHub profile");
            var repoOption = new Option<string>(name:"--repo", description: "A repo name");

            var rootCommand = new RootCommand("CLI app for getting user github activity info");

            rootCommand.AddArgument(usernameArgument);
            rootCommand.AddOption(repoOption);

            rootCommand.SetHandler(async (username, repo) => {
                var result = await Api.GetUserActivity(username, repo);

                Console.WriteLine(result);
            }, usernameArgument, repoOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}