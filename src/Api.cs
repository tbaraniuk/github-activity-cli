using System;
using System.Text.Json;

namespace github_activity.src
{
    public class Repo
    {
        public required string name {get; set;}
    }

    public class UserActivityApiResponse
    {
        public required string type {get; set;}
        public required Repo repo {get; set;}
        public int count {get; set;} = 1;
    }

    public class Api
    {
        public static async Task<string> GetUserActivity(string username, string? repo, string? activityType)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                string originalUri = "https://api.github.com";
                string requestUri = repo?.Length > 0 ? $"/repos/{username}/{repo}/events" : $"/users/{username}/events";

                var response = await client.GetAsync($"{originalUri}{requestUri}");

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var userActivityApiResponse = JsonSerializer.Deserialize<UserActivityApiResponse[]>(jsonResponse);

                if(userActivityApiResponse is null || userActivityApiResponse.Length == 0) {
                    return "This user has no activity yet.\nPlease try another user";
                }

                var formattedList = new List<UserActivityApiResponse>();

                if(activityType is not null)
                {
                    userActivityApiResponse = userActivityApiResponse.Where(item => item.type.ToLower().Contains(activityType.ToLower())).ToArray();
                }

                for(var i = 0; i < userActivityApiResponse.Length; i++)
                {
                    if (i == 0)
                    {
                        formattedList.Add(userActivityApiResponse[i]);

                        continue;
                    }

                    var lastElement = formattedList.Last();

                    if((userActivityApiResponse[i].type == lastElement.type) && (userActivityApiResponse[i].repo.name == lastElement.repo.name))
                    {
                        formattedList.Last().count += 1;
                    }
                    else
                    {
                        formattedList.Add(userActivityApiResponse[i]);
                    }
                }

                return GetUserActivityOutput(formattedList);
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private static string GetUserActivityOutput(List<UserActivityApiResponse> list)
        {
            string result = "";

            foreach(UserActivityApiResponse item in list) {
                switch(item.type)
                {
                    case "CreateEvent":
                        result += $"- Created {item.count} branch{(item.count == 1 ? "" : "es")} or tag{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "DeleteEvent":
                        result += $"- Deleted {item.count} branch{(item.count == 1 ? "" : "es")} or tag{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "ForkEvent":
                        result += $"- Forked a repository {item.repo.name}\n";
                        break;
                    case "PushEvent":
                        result += $"- Pushed {item.count} commit{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "IssueCommentEvent":
                        result += $"- Added issue {item.count} comment{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "IssuesEvent":
                        result += $"- Did {item.count} issue action{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "PullRequestEvent":
                        result += $"- Created {item.count} pull request{(item.count == 1 ? "" : "s")} to {item.repo.name}\n";
                        break;
                    case "WatchEvent":
                        result += $"- Starred {item.repo.name}\n";
                        break;
                    default:
                        result += $"- {item.type} {item.count} to {item.repo.name}\n";
                        break;
                }
            }

            return result;
        }
    }
}