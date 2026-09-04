# RaceDay Database Design

## 1. Purpose

The RaceDay database will store and manage information required by the RaceDay event management system.

The database uses a relational structure to maintain information about users, events, event types, categories, event enrolments and participant results.

The database design supports both Event Organisers and Participants.

## 2. Database Entities

### Users

Stores information about users registered on the RaceDay system.

**Primary Key:** `UserID`

Important attributes include:

* UserID
* FirstName
* LastName
* Email
* Password
* Role

A user can be an Event Organiser or Participant.

### Events

Stores information about RaceDay events created by Event Organisers.

**Primary Key:** `EventID`

Important attributes include:

* EventID
* EventName
* Description
* EventDate
* Location
* Distance
* EventTypeID
* OrganiserID

`OrganiserID` identifies the user who created and manages the event.

### EventTypes

Stores the different types of events supported by RaceDay.

**Primary Key:** `EventTypeID`

Examples include:

* Running
* Walking
* Cycling

### Categories

Stores categories associated with individual events.

**Primary Key:** `CategoryID`

Important attributes include:

* CategoryID
* EventID
* CategoryName
* Description

Each category belongs to an event.

### Enrolments

Stores participant entries into RaceDay events.

**Primary Key:** `EnrolmentID`

Important attributes include:

* EnrolmentID
* UserID
* EventID
* CategoryID

An enrolment connects a participant with an event and their selected category.

### Results

Stores participant race results.

**Primary Key:** `ResultID`

Important attributes include:

* ResultID
* EnrolmentID
* FinishPosition
* FinishTime

Each result is associated with an enrolment.

## 3. Primary Keys

Each entity has a unique primary key:

| Entity     | Primary Key |
| ---------- | ----------- |
| Users      | UserID      |
| Events     | EventID     |
| EventTypes | EventTypeID |
| Categories | CategoryID  |
| Enrolments | EnrolmentID |
| Results    | ResultID    |

Primary keys uniquely identify records within their respective tables.

## 4. Foreign Keys

The database uses foreign keys to maintain relationships between related entities.

| Table      | Foreign Key | References             |
| ---------- | ----------- | ---------------------- |
| Events     | OrganiserID | Users.UserID           |
| Events     | EventTypeID | EventTypes.EventTypeID |
| Categories | EventID     | Events.EventID         |
| Enrolments | UserID      | Users.UserID           |
| Enrolments | EventID     | Events.EventID         |
| Enrolments | CategoryID  | Categories.CategoryID  |
| Results    | EnrolmentID | Enrolments.EnrolmentID |

## 5. Relationships

### Users and Events

One Event Organiser can create many events.

**Relationship:** One-to-Many

`Users → Events`

### EventTypes and Events

One event type can be associated with many events.

**Relationship:** One-to-Many

`EventTypes → Events`

### Events and Categories

One event can contain multiple categories.

**Relationship:** One-to-Many

`Events → Categories`

### Users and Enrolments

One participant can have multiple event enrolments.

**Relationship:** One-to-Many

`Users → Enrolments`

### Events and Enrolments

One event can have multiple participant enrolments.

**Relationship:** One-to-Many

`Events → Enrolments`

### Categories and Enrolments

One category can contain multiple participant enrolments.

**Relationship:** One-to-Many

`Categories → Enrolments`

### Enrolments and Results

An enrolment can have an associated race result.

**Relationship:** One-to-One or One-to-Zero/One, depending on whether a result has been recorded.

`Enrolments → Results`

## 6. Data Integrity

Foreign key relationships will be used to maintain referential integrity between related records.

The database should prevent invalid references, such as:

* An enrolment referencing a participant that does not exist.
* An enrolment referencing an event that does not exist.
* A category referencing an event that does not exist.
* A result referencing an enrolment that does not exist.

Appropriate constraints should also be applied to required fields.

## 7. Relationship to the ERD

The database structure described in this document is represented visually in:

`RaceDay-ERD.png`

The ERD provides a visual representation of the entities, primary keys, foreign keys and relationships used by the RaceDay system.

## 8. Part 1 Database Scope

The database design establishes the foundation for the RaceDay API and application.

The planned API will use these entities to support:

* User registration and authentication
* Event management
* Event category management
* Participant enrolments
* Result recording
* Participant performance history
