# IFS QA Automation Framework

Automated API test suite for the [JSONPlaceholder](https://jsonplaceholder.typicode.com) REST API, built as part of a QA Automation Engineer technical assessment.

---

## Tech Stack

- .NET 8 / C#
- NUnit
- RestSharp
- FluentAssertions 6.12.0
- Allure Reports

---

## Project Structure
```
IFS.ApiTests/
├── IFS.ApiTests/
│   ├── Clients/
│   │   └── ApiClient.cs                # HTTP client wrapper with retry logic and logging
│   ├── Config/
│   │   └── AppSettings.cs              # Configuration model
│   ├── Helpers/
│   │   ├── BaseTest.cs                 # Base test class with setup/teardown
│   │   ├── TestDataLoader.cs           # Loads test data from JSON files
│   │   └── TestLogger.cs               # Request/response logger
│   ├── Models/
│   │   ├── Post.cs                     # Post data model
│   │   └── User.cs                     # User data model
│   ├── TestData/
│   │   ├── PostTestData.json           # Test data for Posts tests
│   │   └── UserTestData.json           # Test data for Users tests
│   ├── Tests/
│   │   ├── Posts/
│   │   │   ├── PostsPositiveTests.cs   # Happy path tests for /posts
│   │   │   └── PostsNegativeTests.cs   # Error case tests for /posts
│   │   └── Users/
│   │       ├── UsersPositiveTests.cs   # Happy path tests for /users
│   │       └── UsersNegativeTests.cs   # Error case tests for /users
│   ├── allureConfig.json               # Allure report configuration
│   ├── appsettings.json                # Base URL, timeout and test settings
│   └── IFS.ApiTests.csproj
└── IFS.ApiTests.sln
```

---

## Features

- ✅ **Retry logic** — failed requests automatically retry 3 times
- ✅ **Request/Response logging** — every request and response logged to console
- ✅ **Data-driven tests** — `[TestCaseSource]` reads test data from JSON files
- ✅ **Response time validation** — all endpoints tested against configurable time limit
- ✅ **Positive/Negative separation** — happy path and error cases in separate classes
- ✅ **Allure Reports** — rich HTML reports with suite, feature, severity and tag annotations
- ✅ **Configurable** — base URL, timeout and response time limit managed via `appsettings.json`
- ✅ **External test data** — test data stored in JSON files, separated from test logic

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

Edit `appsettings.json` to change base URL, timeout or response time limit:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://jsonplaceholder.typicode.com",
    "TimeoutSeconds": 30
  },
  "TestSettings": {
    "MaxResponseTimeMs": 3000
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

---

## Test Coverage

### Posts API `/posts`

| Method | Endpoint | Type | Test Scenarios |
|---|---|---|---|
| GET | `/posts` | Positive | 200 OK, exactly 100 posts, required fields, response time |
| GET | `/posts/{id}` | Positive | Valid IDs (1, 50, 100) return correct data |
| GET | `/posts/{id}` | Negative | Invalid IDs (0, -1, 99999) return 404 |
| POST | `/posts` | Positive | 201 Created, response body matches submitted data |
| POST | `/posts` | Negative | Empty title still accepted by fake API |
| PUT | `/posts/{id}` | Positive | 200 OK, response reflects updated fields |
| DELETE | `/posts/{id}` | Positive | 200 OK |
| GET | `/posts/1/comments` | Positive | Nested resource returns comments |
| GET | `/posts/99999/comments` | Negative | Non-existent post returns empty or 404 |

### Users API `/users`

| Method | Endpoint | Type | Test Scenarios |
|---|---|---|---|
| GET | `/users` | Positive | 200 OK, exactly 10 users, required fields, response time |
| GET | `/users/{id}` | Positive | Valid IDs (1, 5, 10) return correct user |
| GET | `/users/{id}` | Negative | Invalid IDs (0, -1, 99999) return 404 |
| GET | `/users/1/posts` | Positive | Nested resource returns posts for user |
| GET | `/users/99999/posts` | Negative | Non-existent user returns empty or 404 |