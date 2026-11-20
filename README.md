# NearBuy

A mini local marketplace where users can post, browse, and comment on secondhand or unused items — built as a university team web project.

# What it does

NearBuy lets people list items they want to sell (furniture, electronics, textbooks, anything) and lets others browse, search, and ask questions directly on the listing via comments — no messaging system, no accounts required just to browse.

Core loop: post an item → someone finds it → they comment to ask about it → deal happens outside the app.

# Pages
Listings (home) — browse all active listings, filter by category, search by keyword  
Listing detail — full item view (photos, price, description, seller, comments)  
Post a listing — create a new listing (logged-in users)  
My listings — manage your own posts: edit, delete, mark as sold  
Login / Register — basic auth  

# Branching workflow
main — stable, demo-ready state  
dev — active integration branch  
feature/<short-desc> — one branch per task, merged into dev via reviewed MR  

# Data Base ER-diagram
```mermaid
erDiagram
    USER ||--o{ LISTING : "creates"
    CATEGORY ||--o{ LISTING : "contains"
    LISTING ||--o{ LISTING_IMAGE : "has"

    USER {
        int Id PK
        string Name
        string Email UK
        string AvatarUrl
        double Rating
        int ItemsSoldCount
        int MemberSinceYear
        bool IsVerified
    }

    CATEGORY {
        int Id PK
        string Name UK
    }

    LISTING {
        int Id PK
        string Title
        decimal Price
        string Description
        enum Status
        string Location
        string Condition
        string PickupNote
        int Views
        int Inquiries
        datetime CreatedAtUtc
        string Material
        string Dimensions
        string Style
        int UserId FK
        int CategoryId FK
    }

    LISTING_IMAGE {
        int Id PK
        string Url
        bool IsMain
        int ListingId FK
    }
```

# Authentication (Microsoft Entra ID)

- **Tenant ID:** `70a28522-969b-451f-bdb2-abfea3aaa5bf`
- **Client App ID:** `85b29288-c033-4ba4-abff-e6fb49a2ee6e`
- **API App ID:** `6b99ada1-1ca0-4f7f-9a81-86cf13b17134`
- **Scope:** `api://6b99ada1-1ca0-4f7f-9a81-86cf13b17134/access_as_user`

# Verification Screenshots
- [API Scope (`access_as_user`)](docs/entra-id/1a-api-scope.png)
- [App Roles (`User`, `Admin`)](docs/entra-id/1b-app-roles.png)
- [Role Assignments](docs/entra-id/1c-user-assignments.png)

# API protection

All `/api/listings` endpoints need an Entra ID access token (`Authorization: Bearer ...`).
Reading requires the scope `access_as_user`. Changing data requires the scope and a role (`User` or `Admin`).

Policies `ReadListings` and `WriteListings` are defined in `server/Program.cs` and applied to route groups in `server/Features/Listings/ListingEndpoints.cs`.

| Endpoint | Scope | Role | Without token | Without scope or role |
|---|---|---|---|---|
| `GET /api/listings` | `access_as_user` | — | 401 | 403 |
| `GET /api/listings/{id}` | `access_as_user` | — | 401 | 403 |
| `POST /api/listings` | `access_as_user` | `User` or `Admin` | 401 | 403 |
| `PUT /api/listings/{id}` | `access_as_user` | `User` or `Admin` | 401 | 403 |
| `DELETE /api/listings/{id}` | `access_as_user` | `User` or `Admin` | 401 | 403 |

`GET /health` is open.

The client adds the access token only to requests to the API address (`ProfileApi:BaseUrl`, `AuthorizationMessageHandler` in `client/Program.cs`).

# Integration tests

Run `dotnet test server.Tests` (Docker must be running).

| Check | Response | Test |
|---|---|---|
| No token | 401 | `AuthorizationTests.Request_without_token_returns_401_Unauthorized` |
| Token of another API | 401 | `AuthorizationTests.Request_with_token_for_another_API_returns_401_Unauthorized` |
| Expired token | 401 | `ExpiredTokenTests.Request_with_expired_token_returns_401_Unauthorized` |
| Scope without role, changing data | 403 | `RoleTests.Changing_data_without_role_returns_403_Forbidden` |
| Scope and role | 200 / 201 | `RoundTripTests.A_listing_created_through_the_API_is_read_back_from_PostgreSQL` |
