# IFS QA Automation Framework

Automated API test suite for JSONPlaceholder REST API.

## Tech Stack
- .NET 8 / C#
- NUnit
- RestSharp
- FluentAssertions

## Setup
1. Clone the repository
2. Open `IFS.QAAutomation.sln` in Visual Studio 2022
3. Restore NuGet packages

## Configuration
Edit `appsettings.json` to change the base URL or timeout:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://jsonplaceholder.typicode.com",
    "TimeoutSeconds": 30
  }
}
```

## Run Tests
```bash
dotnet test
```

## Test Coverage
- GET /posts — status, count, structure
- GET /posts/{id} — valid ID, 404 for missing
- POST /posts — 201 status, response body validation
- PUT /posts/{id} — update and verify
- DELETE /posts/{id} — 200 status
```

---

## Final Folder Structure
```
IFS.QAAutomation/
└── IFS.ApiTests/
    ├── Config/
    │   └── AppSettings.cs
    ├── Models/
    │   └── Post.cs
    ├── Clients/
    │   └── ApiClient.cs
    ├── Helpers/
    │   └── BaseTest.cs
    ├── Tests/
    │   └── PostsTests.cs
    ├── appsettings.json
    └── README.md