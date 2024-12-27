using System;
using System.CommandLine;
using github_activity.src;

namespace GithubActivity
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var githubActivityOption = new Option<string>("--github-activity", "A github-activity for a user");

            var rootCommand = new RootCommand("CLI app for getting user github activity info");
            rootCommand.AddOption(githubActivityOption);

            rootCommand.SetHandler(async (argument) => {
                var result = await Api.GetUserActivity(argument);

                Console.WriteLine(result);
            }, githubActivityOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}