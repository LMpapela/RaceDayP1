# RaceDay API Endpoint Plan

## 1. Overview

The RaceDay API provides secure RESTful endpoints for managing users, events, event types, categories, participant enrolments and race results.

The API uses JWT authentication and role-based authorization to protect resources.

### Roles

* **Participant** — Can browse events, manage their own enrolments and view their own performance information.
* **Organiser** — Can create and manage events, categories and race results, and manage users.

---

## 2. Authentication Endpoints

| Method | Endpoint             | Access | Purpose                                     |
| ------ | -------------------- | ------ | ------------------------------------------- |
| POST   | `/api/Auth/register` | Public | Register a new participant                  |
| POST   | `/api/Auth/login`    | Public | Authenticate a user and receive a JWT token |

### Registration

`POST /api/Auth/register`

A new user is registered as a Participant. The password is securely hashed before being stored.

### Login

`POST /api/Auth/login`

The user's credentials are validated and a JWT token is returned after successful authentication.

---

## 3. User Endpoints

| Method | Endpoint          | Access                | Purpose                                |
| ------ | ----------------- | --------------------- | -------------------------------------- |
| GET    | `/api/Users`      | Organiser             | View all users                         |
| GET    | `/api/Users/{id}` | Authenticated         | View a user's safe profile information |
| POST   | `/api/Users`      | Organiser             | Create a user                          |
| PUT    | `/api/Users/{id}` | Own user or Organiser | Update user profile                    |
| DELETE | `/api/Users/{id}` | Organiser             | Delete a user                          |

User responses do not expose stored passwords.

Participants can only update their own profiles, while Organisers can update user profiles.

---

## 4. Event Endpoints

| Method | Endpoint           | Access                    | Purpose               |
| ------ | ------------------ | ------------------------- | --------------------- |
| GET    | `/api/Events`      | Authenticated             | View all events       |
| GET    | `/api/Events/{id}` | Authenticated             | View a specific event |
| POST   | `/api/Events`      | Organiser                 | Create an event       |
| PUT    | `/api/Events/{id}` | Organiser and event owner | Update an event       |
| DELETE | `/api/Events/{id}` | Organiser and event owner | Delete an event       |

When an event is created, the authenticated Organiser's user ID is automatically assigned as the event owner.

---

## 5. Event Type Endpoints

| Method | Endpoint               | Access | Purpose                    |
| ------ | ---------------------- | ------ | -------------------------- |
| GET    | `/api/EventTypes`      | Public | View all event types       |
| GET    | `/api/EventTypes/{id}` | Public | View a specific event type |

Event types provide classifications such as Road Running.

---

## 6. Category Endpoints

| Method | Endpoint               | Access                    | Purpose                  |
| ------ | ---------------------- | ------------------------- | ------------------------ |
| GET    | `/api/Categories`      | Authenticated             | View all categories      |
| GET    | `/api/Categories/{id}` | Authenticated             | View a specific category |
| POST   | `/api/Categories`      | Organiser and event owner | Create a category        |
| PUT    | `/api/Categories/{id}` | Organiser and event owner | Update a category        |
| DELETE | `/api/Categories/{id}` | Organiser and event owner | Delete a category        |

Categories are associated with events.

Organisers can only create, update or delete categories belonging to their own events.

---

## 7. Enrolment Endpoints

| Method | Endpoint               | Access                | Purpose                                                            |
| ------ | ---------------------- | --------------------- | ------------------------------------------------------------------ |
| GET    | `/api/Enrolments`      | Authenticated         | Organisers view all enrolments; Participants view their own        |
| GET    | `/api/Enrolments/{id}` | Authenticated         | Organisers can view any enrolment; Participants can view their own |
| POST   | `/api/Enrolments`      | Participant           | Enter an event category                                            |
| DELETE | `/api/Enrolments/{id}` | Participant and owner | Remove the participant's own enrolment                             |

When an enrolment is created, the authenticated Participant's user ID is automatically assigned.

The API validates that:

* The user exists.
* The event exists.
* The category exists.
* The category belongs to the selected event.
* The participant does not already have the same enrolment.

---

## 8. Result Endpoints

| Method | Endpoint                  | Access        | Purpose                                                         |
| ------ | ------------------------- | ------------- | --------------------------------------------------------------- |
| GET    | `/api/Results`            | Organiser     | View all results                                                |
| GET    | `/api/Results/{id}`       | Authenticated | Organisers can view any result; Participants can view their own |
| POST   | `/api/Results`            | Organiser     | Create a participant result                                     |
| PUT    | `/api/Results/{id}`       | Organiser     | Update a result                                                 |
| GET    | `/api/Results/my/history` | Participant   | View the authenticated participant's performance history        |

The API validates that:

* The enrolment exists.
* An enrolment cannot have duplicate results.
* Finish position must be greater than zero.
* Finish time must be greater than zero.

---

## 9. Security Requirements

The RaceDay API implements the following security controls:

### JWT Authentication

Authenticated API requests use JSON Web Tokens.

### Role-Based Authorization

The API distinguishes between Participant and Organiser roles.

### Ownership Authorization

Users can only modify resources they are authorized to manage.

Examples include:

* Organisers can only modify their own events.
* Organisers can only manage categories belonging to their events.
* Participants can only manage their own enrolments.
* Participants can only access their own performance history.
* Participants can only access their own result records.

### Password Security

Passwords are hashed using a salted PBKDF2-based password hashing service before being stored.

### Password Exposure Prevention

API responses do not return stored passwords.

### Input Validation

Request models use validation attributes such as:

* Required fields
* Maximum lengths
* Email validation
* Minimum password length
* Numeric range validation

Additional business-rule validation is performed within the service layer.

---

## 10. HTTP Status Codes

The API uses appropriate HTTP status codes, including:

* **200 OK** — Successful request
* **201 Created** — Resource successfully created
* **204 No Content** — Successful update or deletion
* **400 Bad Request** — Invalid request or business-rule violation
* **401 Unauthorized** — Authentication is missing or invalid
* **403 Forbidden** — Authenticated user does not have permission
* **404 Not Found** — Requested resource does not exist
* **409 Conflict** — Resource conflicts with existing data

---

## 11. Part 1 API Scope

The Part 1 implementation establishes the foundation for the RaceDay platform through:

* RESTful API endpoints
* Entity Framework Core database integration
* SQL Server database
* JWT authentication
* Role-based authorization
* Resource ownership checks
* Password hashing
* Input validation
* Service-layer architecture
* Swagger API documentation and testing
* GitHub version control

Future project parts can extend this foundation with additional functionality and client-side features.
