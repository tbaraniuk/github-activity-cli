## GitHub User Activity

Roadmap.sh project \
link: https://roadmap.sh/projects/github-user-activity

**Run a command for getting project and arguments info**

```shell
dotnet run -- -h
```

**A command for getting user activity:**

```shell
dotnet run github-activity {username}
```

**A command for getting user activity within current repo:**

```shell
dotnet run github-activity {username} --repo {reponame}
```

**A command for getting filtered user activity:**

```shell
dotnet run github-activity {username} --activity-type {type}
```
