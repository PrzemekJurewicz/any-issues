# Any Issues?

A .NET project for managing Issues via GitHub and Bitbucket APIs.
- [Project GitHub](https://github.com/PrzemekJurewicz/any-issues)

## Features

> [!WARNING]
> - Uses both ASP.NET Core minimal APIs and Controllers.
> - This is for demonstration purposes only. In a production application, you should choose one approach for consistency.

- Create, update, and close issues on GitHub and Bitbucket
- Easily extendable for other issue trackers
- Api Versioning with OpenAPI documentation

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- GitHub and/or Bitbucket credentials with appropriate permissions

## Configuration

Preferably, use secrets management for sensitive data like tokens and passwords:

```bash
dotnet user-secrets init
dotnet user-secrets set "GITHUB:OWNER" "your-github-username"
```
… and similarly for other secrets.

Or add them to your `appsettings.json`:

```
GITHUB:OWNER=your-github-username
GITHUB:REPO=your-github-repo
GITHUB:TOKEN=your-github-token

BITBUCKET:WORKSPACE=your-bitbucket-workspace
BITBUCKET:REPO=your-bitbucket-repo
BITBUCKET:USERNAME=your-bitbucket-username
BITBUCKET:APP_PASSWORD=your-bitbucket-app-password
```

## Build and Run

```bash
dotnet build
dotnet run
```

## Usage

API endpoints are available for managing issues on both GitHub and Bitbucket. See the controller classes for details.

## License

MIT
