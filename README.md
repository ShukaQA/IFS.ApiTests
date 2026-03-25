# IFS QA Automation Framework

Automated API test suite for the [JSONPlaceholder](https://jsonplaceholder.typicode.com) REST API, built as part of a QA Automation Engineer technical assessment.

---

## Tech Stack

- .NET 8 / C#
- NUnit
- RestSharp
- FluentAssertions 6.12.0
- Allure Reports
- GitHub Actions (CI/CD)

---

## Project Structure
```
IFS.ApiTests/
├── .github/
│   └── workflows/
│       └── tests.yml         # CI/CD pipeline
├── IFS.ApiTests/
│   ├── Clients/
│   │   └── ApiClient.cs      # HTTP client wrapper with retry logic
│   ├── Config/
│   │   └── AppSettings.cs    # Configuration model
│   ├── Helpers/
│   │   ├── BaseTest.cs       # Base test class with setup/teardown
│   │   └── TestLogger.cs     # Request/response logger
│   ├── Models/
│   │   ├── Post.cs           # Post data model
│   │   └── User.cs           # User data model
│   ├── Tests/
│   │   ├── PostsTests.cs     # Tests for /posts endpoint
│   │   └── UsersTests.cs     # Tests for /users endpoint
│   ├── allureConfig.json     # Allure report configuration
│   ├── appsettings.json      # Base URL and timeout config
│   └── IFS.ApiTests.csproj
└── IFS.ApiTests.sln
```

---

## Features

- ✅ **Retry logic** — failed requests automatically retry 3 times
- ✅ **Request/Response logging** — every request and response is logged to console
- ✅ **Data-driven tests** — `[TestCase]` for multiple inputs per test
- ✅ **Response time validation** — all endpoints tested under 3 seconds
- ✅ **Allure Reports** — rich HTML reports with suite, feature, severity and tag annotations
- ✅ **CI/CD** — GitHub Actions runs tests on every push and pull request
- ✅ **Configurable** — base URL and timeout managed via `appsettings.json`

---

## Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any C# IDE
- [Allure CLI](https://allurereport.org/docs/install/) (optional, for local reports)

### Install Allure CLI (Windows)
```powershell
scoop install allure
```

### Clone and Restore
```bash
git clone https://github.com/ShukaQA/IFS.ApiTests.git
cd IFS.ApiTests
dotnet restore
```

---

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

---

## Run Tests

### Command line
```bash
dotnet test
```

### With detailed output
```bash
dotnet test --verbosity normal
```

---

## Allure Reports

### Generate and open locally
```bash
dotnet test
allure serve IFS.ApiTests/bin/Debug/net8.0/allure-results
```

### Online report (GitHub Pages)
```
https://shukaqa.github.io/IFS.ApiTests/
```

---

## Test Coverage

### Posts API `/posts`
| Method | Endpoint | Test Scenarios |
|---|---|---|
| GET | `/posts` | 200 OK, exactly 100 posts, required fields, response time |
| GET | `/posts/{id}` | Valid IDs (1, 50, 100), invalid IDs (0, -1, 99999), response time |
| POST | `/posts` | 201 Created, response body matches submitted data |
| PUT | `/posts/{id}` | 200 OK, response reflects updated fields |
| DELETE | `/posts/{id}` | 200 OK |
| GET | `/posts/1/comments` | Nested resource returns comments |

### Users API `/users`
| Method | Endpoint | Test Scenarios |
|---|---|---|
| GET | `/users` | 200 OK, exactly 10 users, required fields, response time |
| GET | `/users/{id}` | Valid IDs (1, 5, 10), non-existent ID returns 404 |
| GET | `/users/1/posts` | Nested resource returns posts for user |

---

## CI/CD

Tests run automatically on every push and pull request via GitHub Actions.

- View runs: [GitHub Actions](https://github.com/ShukaQA/IFS.ApiTests/actions)
- Test results uploaded as artifacts after each run
- Allure report published to GitHub Pages automatically