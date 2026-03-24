# Claims Intake & Review Portal

A simple **Claims Management Portal** built using **.NET 8 Web API,
Entity Framework Core, and SQL Server/SQLite**.

This application allows users to: - Create insurance claims - View and
search claims - View claim details - Add/update claim notes - Update
claim status - Delete claims

The project demonstrates **REST API design, EF Core usage, and clean
backend structure**.

------------------------------------------------------------------------

# Tech Stack

**Backend** - .NET 8 Web API - Entity Framework Core - SQL Server /
SQLite

**Frontend** - React (Create, List, Details screens)

**Tools** - Swagger for API testing - Visual Studio / VS Code

------------------------------------------------------------------------

# Features

## Claim Management

-   Create new claims
-   View claim details
-   List all claims (with server-side pagination)
-   Search & Filter claims
-   Sort claims by any column
-   Delete claims

Claim numbers are automatically generated in the format:

CLM-123456

------------------------------------------------------------------------

# Data Model

## Claim

  Field          Type       Description
  -------------- ---------- -----------------------------------------
  Id             GUID       Unique identifier
  ClaimNumber    string     Auto-generated claim number
  MemberName     string     Required
  ProviderName   string     Required
  Amount         decimal    Must be greater than 0
  ServiceDate    Date       Service date
  Status         string     Draft / Submitted / Approved / Rejected
  CreatedAt      DateTime   Created timestamp
  UpdatedAt      DateTime   Last update

## ClaimNote

  Field       Type       Description
  ----------- ---------- --------------------
  Id          GUID       Unique identifier
  ClaimId     GUID       Reference to Claim
  Note        string     1--500 characters
  CreatedAt   DateTime   Note creation time

------------------------------------------------------------------------

# API Endpoints

Base URL

/api/claims

## Get All Claims

GET /api/claims

## Get Paginated Claims

GET /api/claims/paged?page=1&pageSize=20&searchTerm=abc&status=Approved&sortBy=claimNumber&sortDirection=asc

-   **page**: Page number (default: 1)
-   **pageSize**: Items per page (default: 20)
-   **searchTerm**: Filter by Member Name, Provider Name, or Claim Number
-   **status**: Filter by status (Draft, Submitted, Approved, etc.)
-   **sortBy**: Column to sort by
-   **sortDirection**: Sort order (asc/desc)

## Get Claim By Id

GET /api/claims/{id}

Returns claim details along with associated notes.

## Create Claim

POST /api/claims

Example Request:

{ "memberName": "John Smith", "providerName": "City Hospital", "amount":
2500, "serviceDate": "2026-03-10" }

Server automatically generates: - Id - ClaimNumber - Status = Draft -
CreatedAt - UpdatedAt

## Delete Claim

DELETE /api/claims/{id}

Deletes the claim and its related notes.

## Get Claim Notes

GET /api/claims/{id}/notes

Returns all notes associated with the claim.

## Update Claim Notes

PUT /api/claims/{id}/notes

Allows adding, updating, or deleting notes in a single request.

## Update Claim Status

PUT /api/claims/{id}/status

Example request body:

Approved

------------------------------------------------------------------------

# Frontend Screens

## 1. Create Claim

User enters: - Member Name - Provider Name - Amount - Service Date

## 2. Claims List

Displays: - Claim Number - Member Name - Provider - Amount - Status

Includes: - Search - Filter - Sorting

## 3. Claim Details

Shows: - Claim information - Claim notes - Status update - Add/edit
notes

------------------------------------------------------------------------

# Running the Project

## Clone Repository

git clone https://github.com/yourusername/claims-portal.git

cd claims-portal

## Setup Database

dotnet ef database update

## Run Backend

dotnet run

API will run at:

https://localhost:7286

Swagger:

https://localhost:7286/swagger

------------------------------------------------------------------------

# Project Structure

ClaimsApi │ ├── Controllers │ └── ClaimsController.cs │ ├── Models │ ├──
Claim.cs │ └── ClaimNote.cs │ ├── Data │ └── ApplicationDbContext.cs │
├── Migrations │ └── Program.cs

------------------------------------------------------------------------

# Tradeoffs Due to Time Constraint

Since the project was implemented within **3--4 hours**, the following
were simplified:

-   Minimal UI styling
-   No authentication/authorization
-   Limited validation
-   Basic error handling

------------------------------------------------------------------------

# Future Improvements

-   Authentication & Authorization
-   Role-based approvals
-   Unit testing
-   Better UI design
-   Export to CSV/Excel

------------------------------------------------------------------------

# Author

Sakthikumar R
