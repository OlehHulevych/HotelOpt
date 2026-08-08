# HotelOpt — Backend API

ASP.NET Core REST API for HotelOpt, a multi-tenant hotel operations SaaS platform targeting small hotel chains in Central and Eastern Europe.

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 10, ASP.NET Core |
| Database | PostgreSQL (Supabase hosted) via EF Core |
| Identity | ASP.NET Identity (`IdentityUser<Guid>`) + JWT |
| Background Jobs | Hangfire (PostgreSQL store) |
| Real-time | SignalR (notifications + chat) |
| File Storage | Azure Blob Storage |
| AI | Google Gemini Vision API |
| Email | MailKit + Gmail SMTP |
| Billing | Stripe |
| Validation | FluentValidation |
| API Docs | Scalar |

## Architecture

Clean Architecture with four layers:

```
HotelOpt.Domain          → Entities, enums, domain logic
HotelOpt.Application     → Interfaces, services, DTOs, validators
HotelOpt.Infrastructure  → EF Core, repositories, external services
HotelOpt.Api             → Controllers, middleware, hubs, DI wiring
```

Dependencies flow inward only: `Api → Application → Domain`. Infrastructure implements Application interfaces.

### Multi-tenancy

Shared database with `TenantId` on every entity. EF Core global query filters enforce tenant isolation automatically. `ICurrentTenantService` reads `TenantId` from JWT claims on every request.

### Key Patterns

- **Generic Repository** — `IRepository<T> where T : BaseEntity` with `GetAll`, `GetById`, `GetByCondition`, `GetAllPaginated`, `GetByConditionPaginated`, `Add`, `Update`, `Delete`
- **Domain methods** — entities expose behaviour (`booking.CheckIn()`, `room.SetOccupied()`, `task.Complete()`) with guard clauses; setters are private
- **Soft delete** — `BaseEntity.IsDeleted` flag; `Delete()` sets the flag instead of removing the row; global query filters exclude soft-deleted rows
- **Pagination** — `PaginatedResult<T>` returned from all list endpoints
- **Exception middleware** — `ValidationException → 400`, `UnauthorizedException → 401`, `NotFoundException → 404`, `Exception → 500`
- **Audit log** — `AppDbContext.SaveChangesAsync` override snapshots `ChangeTracker` entries and writes one `AuditLog` row per changed entity

## Domain Model

| Entity | Description |
|---|---|
| `Tenant` | Hotel chain; owns subscription plan and Stripe customer |
| `Property` | Individual hotel within a tenant |
| `Room` | Hotel room with status tracking and price per night |
| `Booking` | Guest reservation with check-in/check-out lifecycle |
| `Guest` | Guest profile (passport, contact details) |
| `HouseKeepingTask` | Housekeeping assignment per room |
| `MaintenanceTicket` | Maintenance issue with priority and status |
| `Shift` | Staff work shift |
| `Invoice` | Auto-generated on checkout |
| `Message` | Real-time chat message per property |
| `TaskTemplate` | Reusable housekeeping checklist per room type |
| `TicketAttachment` | File attached to a maintenance ticket |
| `RoomInspection` | AI-generated room inspection result |
| `RoomPhoto` | Room photo stored in Blob Storage |
| `AuditLog` | Change history for all entities |

## API Endpoints

### Auth — `/api/auth`
| Method | Route | Description |
|---|---|---|
| POST | `/register` | Register a new user |
| POST | `/login` | Login, returns JWT + refresh token |
| POST | `/refresh` | Rotate refresh token |
| POST | `/revoke` | Revoke refresh token |
| GET | `/me` | Get current user profile |
| POST | `/avatar` | Upload avatar to Blob Storage |

### Properties — `/api/properties`
CRUD. Create/Update/Delete require `Manager` role. Enforces subscription plan property limits.

### Rooms — `/api/rooms`
CRUD + `GET /api/rooms/property/{propertyId}` with optional `?status=` filter.

### Bookings — `/api/bookings`
| Method | Route | Description |
|---|---|---|
| POST | `/` | Create booking |
| GET | `/property/{propertyId}` | Paginated list with filtering and sorting |
| POST | `/{id}/checkin` | Check in (room → Occupied) |
| POST | `/{id}/checkout` | Check out (room → Cleaning, invoice generated) |
| POST | `/{id}/cancel` | Cancel booking |
| POST | `/{id}/guests/{guestId}` | Add guest to booking |

### Housekeeping Tasks — `/api/tasks`
CRUD + `Start`, `Complete`, `Cancel`, `Reassign` actions. Filtered/sorted paginated list per property.

### Maintenance Tickets — `/api/tickets`
CRUD + `Resolve`, `Close`, `Reassign` actions. File attachments via `POST /api/tickets/{id}/attachments`.

### Shifts — `/api/shifts`
CRUD. Shift scheduling for staff.

### Guests — `/api/guests`
CRUD for guest profiles.

### Invoices — `/api/invoices`
| Method | Route | Description |
|---|---|---|
| GET | `/booking/{bookingId}` | Get invoice for a booking |

### Staff — `/api/users`
| Method | Route | Description | Role |
|---|---|---|---|
| GET | `/` | List all staff in tenant | Any |
| PATCH | `/{id}/role` | Change staff role | Owner |
| DELETE | `/{id}` | Ban user | Owner |
| PATCH | `/{id}/unban` | Unban user | Owner |

### Subscription — `/api/subscription`
| Method | Route | Description | Role |
|---|---|---|---|
| GET | `/status` | Get current plan and status | Any |
| POST | `/subscribe` | Subscribe to a plan | Any |
| DELETE | `/` | Cancel subscription | Owner |
| POST | `/webhook` | Stripe webhook handler | — |

### Export — `/api/export` (Manager only)
- `GET /bookings/{propertyId}` — CSV export
- `GET /tasks/{propertyId}` — CSV export
- `GET /invoices` — CSV export

### Other
- `GET /api/fairness` — Staff fairness scores (weekly task counts)
- `POST /api/inspections` — AI room inspection (photo upload → Gemini Vision)
- `GET /api/inspections/{roomId}` — Inspection history
- `GET /api/shift/{id}/report` — End-of-shift report
- `GET /api/audit`, `GET /api/audit/{entityName}/{entityId}` — Audit log (Manager only)
- `GET /api/messages/{propertyId}` — Chat message history

### SignalR Hubs
- `/hubs/notifications` — Tenant-scoped alerts (overdue tasks, tickets, understaffed shifts)
- `/hubs/chat` — Property-scoped real-time chat

## Background Jobs (Hangfire)

| Job | Schedule | Description |
|---|---|---|
| Auto-assignment | Daily | Round-robin assign unassigned housekeeping tasks; skips staff with ≥ 25 tasks/week |
| Smart alerts | Hourly | Detects overdue tasks, overdue tickets, understaffed shifts → pushes SignalR notifications + emails |

Hangfire dashboard available at `/hangfire`.

## Subscription Plans

| Plan | Properties | Duration |
|---|---|---|
| Trial | 1 | 30 days |
| Basic | 3 | Monthly |
| Pro | Unlimited | Monthly |

`SubscriptionMiddleware` returns `402` for `Locked` or `Cancelled` tenants. Stripe webhooks handle `invoice.paid`, `invoice.payment_failed`, and `customer.subscription.deleted`.

## Getting Started

### Prerequisites
- .NET 10 SDK
- PostgreSQL database
- Azure Blob Storage account
- Stripe account (test mode is fine)
- Gmail account with app password
- Google Gemini API key

### Configuration

All secrets are stored via .NET User Secrets. Run the following to configure:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your-postgres-connection-string>"
dotnet user-secrets set "Jwt:Key" "<your-jwt-secret>"
dotnet user-secrets set "AzureBlobStorage:ConnectionString" "<your-azure-connection-string>"
dotnet user-secrets set "Stripe:SecretKey" "<your-stripe-secret-key>"
dotnet user-secrets set "Stripe:WebhookSecret" "<your-stripe-webhook-secret>"
dotnet user-secrets set "Email:SenderEmail" "<your-gmail-address>"
dotnet user-secrets set "Email:Password" "<your-gmail-app-password>"
dotnet user-secrets set "Gemini:ApiKey" "<your-gemini-api-key>"
```

### Run

```bash
dotnet restore
dotnet ef database update --project HotelOpt.Infrastructure --startup-project HotelOpt.Api
dotnet run --project HotelOpt.Api
```

API will be available at `http://localhost:5092`.  
Scalar docs: `http://localhost:5092/scalar`  
Hangfire dashboard: `http://localhost:5092/hangfire`

### Docker

```bash
docker build -t hotelopt-api .
docker run -p 8080:8080 --env-file .env hotelopt-api
```

### Seed Data

A seed script is available at `Scripts/seed.sql`. Test users (password: `Test@1234`):

| Email | Role |
|---|---|
| anna.koval@hotel.com | Manager |
| ivan.petrenko@hotel.com | Staff |
| maria.bondar@hotel.com | Staff |
| dmytro.kravchenko@hotel.com | Staff |

## Role-Based Access

| Role | Capabilities |
|---|---|
| Owner | Full access + staff management + billing |
| Manager | Write access to all operational data + CSV export + audit log |
| Staff | Read access + task/ticket status updates |
