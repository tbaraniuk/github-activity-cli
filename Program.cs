using System;
using System.CommandLine;
using github_activity.src;

namespace GithubActivity
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var usernameArgument = new Argument<string>(name: "username", description: "A username for a user of GitHub profile");
            var repoOption = new Option<string>(name: "--repo", description: "A repo name");
            var activityTypeOption = new Option<string>(name:"--activity-type", description: "Filter by activity type");

            var rootCommand = new RootCommand("CLI app for getting user github activity info");

            rootCommand.AddArgument(usernameArgument);
            rootCommand.AddOption(repoOption);
            rootCommand.AddOption(activityTypeOption);

            rootCommand.SetHandler(async (username, repo, activityType) => {
                var result = await Api.GetUserActivity(username, repo, activityType);

                Console.WriteLine(result);
            }, usernameArgument, repoOption, activityTypeOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}