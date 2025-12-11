# Elastic Sentinel Minimal API Documentation

## Overview
Elastic Sentinel now includes a comprehensive REST API built with .NET 10 Minimal APIs, running alongside the existing Razor Pages UI.

## API Endpoints

### Base URL
- **Development**: `https://localhost:7293/api` or `http://localhost:5070/api`
- **Swagger Documentation**: `https://localhost:7293/api/docs`

### Authentication
Currently using open CORS policy. JWT authentication can be added in future phases.

---

## Queries API (`/api/queries`)

Manage Elasticsearch query configurations.

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/queries` | Get all Elasticsearch queries |
| GET | `/api/queries/{id}` | Get a specific query by ID |
| POST | `/api/queries` | Create a new query |
| PUT | `/api/queries/{id}` | Update an existing query |
| DELETE | `/api/queries/{id}` | Delete a query |

### Example Request (Create Query)
```json
POST /api/queries
{
  "queryName": "Error Log Monitor",
  "queryDescription": "Monitors application error logs",
  "isDynamic": true,
  "elasticDynamicQueryDetailId": 1,
  "elasticDynamicQueryResponseDetailId": 1
}
```

---

## Connectors API (`/api/connectors`)

Manage email and Teams notification connectors.

### Email Connectors (`/api/connectors/email`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/connectors/email` | Get all email connectors |
| GET | `/api/connectors/email/{id}` | Get specific email connector |
| POST | `/api/connectors/email` | Create new email connector |
| PUT | `/api/connectors/email/{id}` | Update email connector |
| DELETE | `/api/connectors/email/{id}` | Delete email connector |

### Email Recipients (`/api/connectors/email-details`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/connectors/email-details` | Get all email recipients |
| GET | `/api/connectors/email-details/{id}` | Get specific recipient |
| POST | `/api/connectors/email-details` | Add new recipient |
| PUT | `/api/connectors/email-details/{id}` | Update recipient |
| DELETE | `/api/connectors/email-details/{id}` | Delete recipient |

### Teams Connectors (`/api/connectors/teams`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/connectors/teams` | Get all Teams connectors |
| GET | `/api/connectors/teams/{id}` | Get specific Teams connector |
| POST | `/api/connectors/teams` | Create new Teams connector |
| PUT | `/api/connectors/teams/{id}` | Update Teams connector |
| DELETE | `/api/connectors/teams/{id}` | Delete Teams connector |

### Example Request (Create Email Connector)
```json
POST /api/connectors/email
{
  "name": "Production Alerts",
  "fromEmail": "alerts@company.com",
  "primarySMTPServer": "smtp.gmail.com",
  "smtpPort": 587,
  "username": "user@company.com",
  "password": "app-password",
  "isEnabled": true
}
```

---

## Scheduler API (`/api/scheduler`)

Manage alert scheduler configurations and execution details.

### Scheduler Configs (`/api/scheduler/configs`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/scheduler/configs` | Get all scheduler configs |
| GET | `/api/scheduler/configs/{id}` | Get specific config |
| POST | `/api/scheduler/configs` | Create new scheduler |
| PUT | `/api/scheduler/configs/{id}` | Update scheduler |
| DELETE | `/api/scheduler/configs/{id}` | Delete scheduler |

### Scheduler Details (`/api/scheduler/details`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/scheduler/details` | Get all scheduler details |
| GET | `/api/scheduler/details/{id}` | Get specific detail |
| POST | `/api/scheduler/details` | Create scheduler detail |
| PUT | `/api/scheduler/details/{id}` | Update detail |
| DELETE | `/api/scheduler/details/{id}` | Delete detail |

### Example Request (Create Scheduler)
```json
POST /api/scheduler/configs
{
  "schedulerName": "Hourly Error Check",
  "schedulerGroup": "ErrorMonitoring",
  "elasticQueryId": 1,
  "isEnabled": true,
  "cronExp": "0 0 * * * ?"
}
```

---

## Notification Templates API (`/api/templates`)

Manage notification message templates.

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/templates` | Get all templates |
| GET | `/api/templates/{id}` | Get specific template |
| POST | `/api/templates` | Create new template |
| PUT | `/api/templates/{id}` | Update template |
| DELETE | `/api/templates/{id}` | Delete template |

### Example Request (Create Template)
```json
POST /api/templates
{
  "templateName": "Error Alert Template",
  "templateContent": "<html><body><h1>Error Alert</h1>...</body></html>",
  "isEnabled": true
}
```

---

## Response Formats

### Success Response (200 OK)
```json
{
  "elasticQueryId": 1,
  "queryName": "Error Log Monitor",
  "queryDescription": "Monitors application error logs",
  "isDynamic": true
}
```

### Created Response (201 Created)
```json
{
  "elasticQueryId": 5,
  "queryName": "New Query",
  "queryDescription": "Description"
}
```

### Error Response (404 Not Found)
```json
{
  "message": "Query with ID 99 not found"
}
```

### Error Response (400 Bad Request)
```json
{
  "message": "Query name is required"
}
```

---

## CORS Configuration

The API allows all origins, methods, and headers for development purposes.

```csharp
Policy: AllowAll
- Origins: *
- Methods: *
- Headers: *
```

**Production Note**: Restrict CORS to specific domains before deploying to production.

---

## Testing with Swagger

1. Start the application: `dotnet run`
2. Navigate to: `https://localhost:7293/api/docs`
3. Use the interactive Swagger UI to test endpoints
4. Click "Try it out" on any endpoint to execute requests

---

## Testing with cURL

### Get all queries
```bash
curl -X GET "https://localhost:7293/api/queries" -H "accept: application/json"
```

### Create a query
```bash
curl -X POST "https://localhost:7293/api/queries" \
  -H "Content-Type: application/json" \
  -d '{
    "queryName": "Test Query",
    "queryDescription": "Test Description",
    "isDynamic": false,
    "elasticDynamicQueryDetailId": 1,
    "elasticDynamicQueryResponseDetailId": 1
  }'
```

### Get specific query
```bash
curl -X GET "https://localhost:7293/api/queries/1" -H "accept: application/json"
```

---

## Future Enhancements

### Phase 5 (Planned)
- ✅ JWT Authentication
- ✅ API Key support
- ✅ Rate limiting
- ✅ Request validation with FluentValidation
- ✅ Versioning (v1, v2)

### Phase 6 (Planned)
- Angular SPA consuming these APIs
- Real-time updates via SignalR
- Bulk operations support

---

## Architecture

```
Presentation/
└── API/
    ├── Endpoints/          # Minimal API endpoint definitions
    │   ├── ElasticQueriesEndpoints.cs
    │   ├── ConnectorsEndpoints.cs
    │   ├── SchedulerEndpoints.cs
    │   └── NotificationTemplatesEndpoints.cs
    ├── Filters/            # Endpoint filters
    │   └── ValidationFilter.cs
    ├── Middleware/         # Custom middleware
    │   └── GlobalExceptionHandler.cs
    └── ApiEndpointsExtensions.cs  # Registration
```

---

## Notes

- All endpoints use async/await for better performance
- Entity Framework Core handles database operations
- Global exception handler catches unhandled errors
- Swagger UI provides interactive documentation
- Both Razor Pages and APIs run on the same port
- SignalR hub remains available at `/jobshub`

---

## Migration from Razor Pages

The application now supports **dual hosting**:
- **Razor Pages UI**: `https://localhost:7293/` 
- **REST API**: `https://localhost:7293/api/`
- **Swagger Docs**: `https://localhost:7293/api/docs`

Both interfaces share the same:
- Database (SQLite)
- Business logic (Application layer)
- Background jobs (Quartz.NET)
- Configuration
