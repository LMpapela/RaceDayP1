# RaceDayP3 — Event Management Web Application

## Overview

RaceDay is a web-based event management platform designed for South Africa’s running, walking, and cycling community.

Part 3 implements the MVC web application that consumes the RaceDay REST API developed in Part 2. It provides a user interface for account access and event discovery while keeping API communication separate from the presentation layer.

## Features

- Responsive home page and navigation.
- Participant registration and login interfaces.
- Cookie-based authentication for the MVC application.
- JWT bearer authentication for protected API requests.
- Session-based storage of the API token.
- Event discovery using data retrieved from the API.
- Event details pages.
- Light and dark appearance options.
- Basic validation and API error handling.

*Only features implemented and verified in the submitted code should be described as complete.*

## Technology Stack

| Component | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC |
| User interface | Razor Views, HTML, CSS, JavaScript |
| Styling | Bootstrap and custom CSS |
| API communication | `HttpClient` |
| Authentication | Cookies and JWT bearer tokens |
| Session management | ASP.NET Core Session |
| Backend integration | RaceDay REST API |

## YouTube Demonstration
https://youtu.be/-p1CWiCNPrI?si=QsXkHMoUFSDukOhP

## Architecture

RaceDayP3 follows the Model-View-Controller (MVC) pattern.

- **Models:** Represent application data and form inputs.
- **Views:** Render the user interface using Razor.
- **Controllers:** Handle requests and coordinate application logic.
- **Services:** Communicate with the RaceDay REST API over HTTP.
- **Configuration:** Defines application settings, including the API address.

The MVC application accesses data through the API rather than connecting directly to its database.

## Prerequisites

- Microsoft Visual Studio with the ASP.NET and web development workload.
- The .NET SDK version required by the project.
- The RaceDay REST API and its database configured and running.

## Configuration

Set the API base URL in `appsettings.json`:

```json id="4a2f5c"
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7198/"
  }
}
```

Change the URL if your API uses a different address or port.

## Running the Application

1. Open the RaceDayP3 solution in Visual Studio.
2. Configure the API base URL.
3. Start the RaceDay API and confirm it is running.
4. Set RaceDayP3 as the startup project.
5. Build the solution and resolve any errors.
6. Run the MVC application.
7. Test registration, login, event discovery, and event details.

## Testing Checklist

- [ ] Solution builds successfully.
- [ ] Home page loads correctly.
- [ ] Participant registration works.
- [ ] Login and logout work.
- [ ] Events load from the API.
- [ ] Event details display correctly.
- [ ] Authentication and error handling behave as expected.

## Security

- Protected API requests use JWT bearer tokens.
- MVC authentication uses secure cookie configuration.
- Session cookies are configured as HTTP-only.
- Password handling is delegated to the API.
- Secrets and credentials should not be committed to source control.
- Access permissions must be enforced by the API.

## Submission Evidence

**GitHub repository:** [Insert repository URL]

**Demonstration video:** [Insert unlisted YouTube URL]

**CI/CD evidence:** [Insert the required successful build screenshot]

The demonstration video should explain the application structure, key functionality, API integration, authentication, and design decisions. Use your own voice for the presentation, as required by the assignment.

## Conclusion

RaceDayP3 provides the MVC interface for the RaceDay platform, integrating account access and event discovery with the REST API. Its layered structure separates the user interface, application logic, and API communication.

The final submission should include the verified source code and all documentation, screenshots, video evidence, and additional deliverables specified in the assignment brief.
