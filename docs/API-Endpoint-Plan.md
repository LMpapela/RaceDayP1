# RaceDay API Endpoint Plan

## 1. Authentication

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/auth/register | Registers a new user as either an Organiser or Participant. | None (Public) | FirstName, LastName, Email, Password, Role | 201 Created - User registered; 400 Bad Request - Invalid data; 409 Conflict - Email already exists |
| POST | /api/auth/login | Authenticates a registered user and establishes their authenticated session. | None (Public) | Email, Password | 200 OK - Login successful; 401 Unauthorized - Invalid credentials |

## 2. User Profile

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/users/profile | Returns the profile of the currently authenticated user. | Any logged-in user | None | 200 OK - User profile; 401 Unauthorized - Not authenticated |
| PUT | /api/users/profile | Updates the profile information of the currently authenticated user. | Any logged-in user | FirstName, LastName, Email | 200 OK - Updated profile; 400 Bad Request - Invalid data; 401 Unauthorized - Not authenticated |

## 3. Events

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events | Returns a list of upcoming RaceDay events. | None (Public) | None | 200 OK - List of events |
| GET | /api/events/{id} | Returns details for a specific event. | None (Public) | None | 200 OK - Event details; 404 Not Found - Event does not exist |
| POST | /api/events | Creates a new event owned by the authenticated organiser. | Organiser | EventName, Description, EventDate, Location, Distance, EventTypeID | 201 Created - Event created; 400 Bad Request - Invalid data; 401 Unauthorized - Not authenticated; 403 Forbidden - Participant attempted access |
| PUT | /api/events/{id} | Updates an existing event belonging to the authenticated organiser. | Organiser | EventName, Description, EventDate, Location, Distance, EventTypeID | 200 OK - Event updated; 404 Not Found - Event does not exist; 403 Forbidden - Not owner |
| DELETE | /api/events/{id} | Deletes an event belonging to the authenticated organiser. | Organiser | None | 204 No Content - Event deleted; 404 Not Found - Event does not exist; 403 Forbidden - Not owner |

## 4. Event Types

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/eventtypes | Returns all available event types such as Run, Walk, and Cycle. | None (Public) | None | 200 OK - List of event types |

## 5. Categories

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| GET | /api/events/{eventId}/categories | Returns all categories available for a specific event. | None (Public) | None | 200 OK - List of categories; 404 Not Found - Event does not exist |
| POST | /api/events/{eventId}/categories | Creates a category for an event. | Organiser | CategoryName, Description | 201 Created - Category created; 404 Not Found - Event does not exist; 403 Forbidden - Not event organiser |
| PUT | /api/categories/{id} | Updates an existing event category. | Organiser | CategoryName, Description | 200 OK - Category updated; 404 Not Found - Category does not exist; 403 Forbidden - Not authorised |
| DELETE | /api/categories/{id} | Deletes an event category. | Organiser | None | 204 No Content - Category deleted; 404 Not Found - Category does not exist; 403 Forbidden - Not authorised |

## 6. Event Enrolments

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/enrolments | Enrols the authenticated participant into an event using a selected category. | Participant | EventID, CategoryID | 201 Created - Enrolment created; 400 Bad Request - Invalid data; 401 Unauthorized - Not authenticated; 403 Forbidden - Organiser attempted access; 409 Conflict - Already enrolled |
| GET | /api/enrolments/my | Returns all event enrolments belonging to the authenticated participant. | Participant | None | 200 OK - Participant enrolments; 401 Unauthorized - Not authenticated; 403 Forbidden - Organiser attempted access |
| GET | /api/events/{eventId}/enrolments | Returns all participants enrolled in an organiser's event. | Organiser | None | 200 OK - Event enrolments; 404 Not Found - Event does not exist; 403 Forbidden - Not event organiser |

## 7. Results

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
|---|---|---|---|---|---|
| POST | /api/results | Records a participant's finishing position and finish time after an event. | Organiser | EnrolmentID, FinishPosition, FinishTime | 201 Created - Result recorded; 400 Bad Request - Invalid data; 403 Forbidden - Not authorised |
| PUT | /api/results/{id} | Updates a participant's recorded result. | Organiser | FinishPosition, FinishTime | 200 OK - Result updated; 404 Not Found - Result does not exist; 403 Forbidden - Not authorised |
| GET | /api/results/my | Returns the authenticated participant's personal results. | Participant | None | 200 OK - Personal results; 401 Unauthorized - Not authenticated; 403 Forbidden - Organiser attempted access |
| GET | /api/events/{eventId}/results | Returns all results for an event. | Organiser | None | 200 OK - Event results; 404 Not Found - Event does not exist; 403 Forbidden - Not event organiser |