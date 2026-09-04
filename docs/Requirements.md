# RaceDay Part 1 Requirements

## 1. System Overview

RaceDay is a web-based event management system for the South African road running, walking and cycling community.

The system will support two primary types of users:

- Event Organisers
- Participants

## 2. Event Organiser Requirements

Event Organisers must be able to:

- Create events.
- Update event information.
- View existing events.
- Manage event categories.
- Manage participant entries.
- Record participant results.
- View participant results for their events.

## 3. Participant Requirements

Participants must be able to:

- Register for an account.
- Log into the system.
- Browse available events.
- View event information.
- Enter available events.
- View their event entries.
- View their race results.
- View their personal performance history.

## 4. Event Requirements

Each event should contain relevant information such as:

- Event name
- Event date
- Event location
- Event type
- Event category information
- Entry information
- Participant information
- Results

## 5. Result Requirements

The system must support the recording and retrieval of participant results.

Results should be associated with:

- A participant
- An event
- An event category

Participant performance history should be available to the relevant participant.

## 6. Database Requirements

The system requires a relational database to store and manage:

- Users
- Events
- Event types
- Categories
- Event entries
- Results

Relationships between these entities must be represented in the database design.

## 7. API Requirements

The system will use an API-driven architecture.

The API will provide endpoints for functionality such as:

- User management
- Event management
- Category management
- Event entries
- Results

The planned endpoints are documented separately in:

`API-Endpoint-Plan.md`

## 8. Security Requirements

The system should protect user and application data through appropriate security practices.

These include:

- User authentication
- Authorisation where required
- Input validation
- Appropriate handling of user credentials
- Protection of API resources

## 9. Part 1 Development Focus

Part 1 focuses on establishing the foundation of the RaceDay application.

The following should be completed before or during the initial implementation:

1. Project requirements
2. System planning
3. Database design
4. Entity Relationship Diagram
5. API endpoint planning
6. Project structure
7. Initial application implementation
8. Version control through meaningful GitHub commits

## 10. Version Control

GitHub will be used to track the development of the project.

Development must be represented through meaningful commits that demonstrate genuine progress rather than artificial or trivial changes.