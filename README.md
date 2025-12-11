# Elastic Sentinel

A comprehensive monitoring and alerting system that integrates with Elasticsearch to provide real-time query execution, alert scheduling, and multi-channel notifications.

## 📋 Overview

Elastic Sentinel is an ASP.NET Core application that enables automated monitoring of Elasticsearch indices with configurable alerts and notifications. Built with Clean Architecture principles, it supports scheduled queries, dynamic alert rules, and notifications via email and Microsoft Teams.

**Framework**: .NET 10.0 | **Architecture**: Clean Architecture | **Current Version**: 2.0.0

## ✨ Features

- **Elasticsearch Integration** - Execute dynamic queries against Elasticsearch clusters
- **Alert Scheduling** - Quartz.NET-based job scheduler for automated alert execution
- **Multi-Channel Notifications** - Email (SMTP) and Microsoft Teams webhooks
- **Template Engine** - Scriban-based templating for customizable alert messages
- **Real-time Updates** - SignalR hub for live job status updates
- **Audit Trail** - Document processing history and alert tracking
- **Clean Architecture** - Domain-driven design with proper separation of concerns
- **Handler Pattern** - CQRS-style command/query handlers (MediatR alternative)
- **FluentValidation** - Request validation infrastructure
- **Result Pattern** - Type-safe error handling

## 🏗️ Architecture

### Project Structure

```
ElasticSentinel/
├── Domain/                     # Enterprise Business Rules (no dependencies)
│   ├── Common/                 # Domain constants
│   ├── Entities/              # 13 database entities
│   ├── Enums/                 # Domain enumerations
│   ├── Exceptions/            # Domain-specific exceptions
│   └── Interfaces/            # Domain contracts
│
├── Application/                # Application Business Rules (depends on Domain)
│   ├── Common/
│   │   ├── Abstractions/      # Handler infrastructure (IHandler, ICommandHandler, etc.)
│   │   ├── Behaviors/         # Validation attributes
│   │   ├── Interfaces/        # Service interfaces
│   │   └── Models/            # DTOs, Result Pattern (Error, Result<T>)
│   ├── Features/              # Business logic by feature (7 handlers)
│   │   ├── ElasticQueries/    # Query execution
│   │   ├── Alerts/            # Alert rendering
│   │   ├── Documents/         # Document processing
│   │   └── Notifications/     # Multi-channel notifications
│   └── Validators/            # FluentValidation validators (6 validators)
│
├── Infrastructure/             # External Concerns (depends on Application + Domain)
│   ├── Persistence/           # EF Core, DbContext, Migrations
│   ├── BackgroundJobs/        # Quartz.NET jobs, SignalR hub
│   └── DependencyInjection.cs
│
├── Presentation/               # UI Layer (Razor Pages)
│   ├── Pages/                 # CRUD pages for configuration
│   └── wwwroot/               # Static assets
│
└── tests/
    └── ElasticSentinel.Tests/ # xUnit, Moq, FluentAssertions
```

### Dependency Flow
```
Domain (Core) ← Application ← Infrastructure ← Presentation
```

### Key Components

**Handlers** (Application Layer):
- `ExecuteElasticQueryHandler` - Elasticsearch query execution
- `RenderAlertMessageHandler` - Alert message templating
- `ProcessDocumentsHandler` - Document processing and audit
- `RenderNotificationHandler` - Multi-channel notification orchestration
- `SendEmailNotificationHandler` - Email delivery
- `SendTeamsNotificationHandler` - Teams webhook delivery

**Background Jobs** (Infrastructure Layer):
- `AlertSchedulerJob` - Main scheduler triggering alert processing
- `ElasticQueryManagerJob` - Executes configured queries
- `NotifyManagerJob` - Sends notifications based on results

**Entities** (Domain Layer):
- `AlertSchedulerConfig`, `AlertSchedulerDetail` - Alert configurations
- `ElasticConfiguration` - Elasticsearch connections
- `ElasticQuery`, `ElasticDynamicQuery*` - Query definitions (5 entities)
- `EmailConnector`, `EmailConnectorDetail` - Email settings
- `MSTeamsConnector` - Teams webhook settings
- `NotificationTemplate` - Message templates
- `DocumentsProcessingDetail` - Processing audit

## 🚀 Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQLite (included) or SQL Server
- Elasticsearch cluster (7.x or 8.x)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ChandreshRao/elastic-sentinel.git
   cd elastic-sentinel
   ```

2. **Configure the application**
   ```bash
   # Copy example configuration
   cp appsettings.example.json appsettings.json
   
   # Edit appsettings.json with your actual values
   # OR use User Secrets (recommended for development)
   dotnet user-secrets set "ElasticsearchSettings:Password" "your-password"
   dotnet user-secrets set "EmailSettings:SmtpPassword" "smtp-password"
   ```

3. **Set up the database**
   ```bash
   # Install EF Core CLI tools (if not already installed)
   dotnet tool install --global dotnet-ef
   
   # Apply migrations to create database schema
   dotnet ef database update
   
   # Optional: Populate with initial data
   # Customize docs/examples/onetimescript.example.sql first
   sqlite3 datasource/ElasticSentinel.db < your-custom-init.sql
   ```

4. **Run the application**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   
   # Navigate to https://localhost:5001 (or http://localhost:5000)
   ```

### Docker Deployment

```bash
# Build image
docker build -t elastic-sentinel:latest .

# Run container
docker run -d \
  -p 5000:80 \
  -p 5001:443 \
  -v $(pwd)/datasource:/app/datasource \
  -v $(pwd)/logs:/app/logs \
  -e ElasticsearchSettings__Password=your-password \
  elastic-sentinel:latest
```

## 🌐 REST API

Elastic Sentinel includes a complete REST API for programmatic access to all resources.

### API Endpoints

| Resource | Base Path | Methods |
|----------|-----------|---------|
| Queries | `/api/queries` | GET, POST, PUT, DELETE |
| Email Connectors | `/api/connectors/email` | GET, POST, PUT, DELETE |
| Email Recipients | `/api/connectors/email-details` | GET, POST, PUT, DELETE |
| Teams Connectors | `/api/connectors/teams` | GET, POST, PUT, DELETE |
| Scheduler Configs | `/api/scheduler/configs` | GET, POST, PUT, DELETE |
| Scheduler Details | `/api/scheduler/details` | GET, POST, PUT, DELETE |
| Templates | `/api/templates` | GET, POST, PUT, DELETE |

### Example Usage

```bash
# Get all queries
curl -X GET "http://localhost:5000/api/queries"

# Create a query
curl -X POST "http://localhost:5000/api/queries" \
  -H "Content-Type: application/json" \
  -d '{"queryName":"Test","queryDescription":"Test Query"}'
```

📖 **Full API Documentation**: See [docs/API-Documentation.md](docs/API-Documentation.md)

**Note**: Swagger/OpenAPI documentation temporarily disabled due to .NET 10 compatibility. Use cURL, Postman, or other API clients.

## ⚙️ Configuration

### Configuration Files

- **appsettings.json** - Main configuration (⚠️ DO NOT commit with real credentials)
- **appsettings.Development.json** - Development overrides
- **appsettings.example.json** - Template with placeholder values (safe to commit)
- **User Secrets** - Recommended for local development (stored outside project)
- **Environment Variables** - Recommended for production deployment

### Key Settings

#### Elasticsearch
```json
{
  "ElasticsearchSettings": {
    "Host": "https://your-elasticsearch-host.com",
    "Port": 9200,
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

#### Email (SMTP)
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "FromEmail": "alerts@yourdomain.com",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

**Gmail Setup**: Enable 2FA and use an [App Password](https://support.google.com/accounts/answer/185833).

#### Microsoft Teams
```json
{
  "TeamsSettings": {
    "WebhookUrl": "https://your-org.webhook.office.com/webhookb2/YOUR-WEBHOOK-ID"
  }
}
```

**Teams Setup**: Create an [Incoming Webhook](https://learn.microsoft.com/en-us/microsoftteams/platform/webhooks-and-connectors/how-to/add-incoming-webhook) in your Teams channel.

#### Database
```json
{
  "ConnectionStrings": {
    "SentinelDb": "Data source=datasource/ElasticSentinel.db"
  }
}
```

### Using User Secrets (Development)

```bash
# Set individual secrets
dotnet user-secrets set "ElasticsearchSettings:Password" "your-password"
dotnet user-secrets set "EmailSettings:Password" "smtp-password"
dotnet user-secrets set "TeamsSettings:WebhookUrl" "your-webhook-url"

# List all secrets
dotnet user-secrets list

# Remove a secret
dotnet user-secrets remove "ElasticsearchSettings:Password"

# Clear all secrets
dotnet user-secrets clear
```

### Using Environment Variables (Production)

```bash
# Linux/Mac
export ElasticsearchSettings__Password="your-password"
export EmailSettings__Password="smtp-password"

# Windows PowerShell
$env:ElasticsearchSettings__Password="your-password"
$env:EmailSettings__Password="smtp-password"

# Docker
docker run -e ElasticsearchSettings__Password=your-password ...
```

## 🔐 Security

### Protected Files (Never Commit)

- `appsettings.json`, `appsettings.Development.json` - Configuration with real credentials
- `datasource/*.db*` - SQLite database files
- `logs/*.log` - Application logs
- `bin/`, `obj/` - Build artifacts

### Safe to Share

- `appsettings.example.json` - Template with placeholders
- `docs/examples/` - Example scripts and templates
- All source code (`.cs`, `.cshtml`, etc.)
- Project files (`*.csproj`, `*.sln`)

### Best Practices

1. **Never commit credentials** - Use User Secrets or Environment Variables
2. **Use HTTPS** - Always in production
3. **Rotate secrets regularly** - API keys, passwords, webhook URLs
4. **Limit permissions** - Use least-privilege principle for service accounts
5. **Audit logs** - Review application logs for suspicious activity
6. **Update dependencies** - Keep NuGet packages current for security patches

### Before Committing Checklist

- [ ] No passwords or API keys in code
- [ ] `.gitignore` is up to date
- [ ] `appsettings.json` contains only example/default values
- [ ] Database files are excluded
- [ ] Log files are excluded
- [ ] Secrets are in User Secrets or Environment Variables

## 📦 Dependencies

### Core Packages

- **Microsoft.EntityFrameworkCore.Sqlite** 10.0.0
- **Microsoft.EntityFrameworkCore.SqlServer** 10.0.0
- **Quartz.AspNetCore** 3.13.1 - Job scheduling
- **Scriban** 5.12.0 - Template engine
- **Serilog.AspNetCore** 8.0.3 - Structured logging
- **FluentValidation.DependencyInjectionExtensions** 11.10.0 - Request validation

### Test Packages

- **xUnit** 2.9.3
- **Moq** 4.20.72
- **FluentAssertions** 8.8.0

### Database Schema

The application uses Entity Framework Core with the following main tables:

- **elastic_configuration** - Elasticsearch connection settings
- **elastic_query** - Query definitions
- **elastic_dynamic_query_source** - Dynamic query configurations
- **elastic_dynamic_query_request_detail** - Request specifications
- **elastic_dynamic_query_response_detail** - Response mappings
- **elastic_dynamic_query_response_structure** - Field mappings
- **email_connector** - SMTP settings
- **email_connector_detail** - Email recipients and subjects
- **teams_connector** - Teams webhook configurations
- **notification_template** - Message templates
- **alert_scheduler_config** - Alert schedule configurations
- **alert_scheduler_detail** - Alert execution history
- **documents_processing_detail** - Processing audit trail

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover

# Run specific test
dotnet test --filter "FullyQualifiedName~RenderNotificationHandlerTests"
```

### Test Structure

- **Unit Tests** - Handler logic, validation, result patterns
- **Integration Tests** - Database operations, external services
- **Handler Tests** - Command/query handler execution

**Current Coverage**: Test infrastructure ready, expand coverage in future phases

## 📊 Development

### Building

```bash
dotnet restore
dotnet build
```

### Running

```bash
# Development mode (hot reload)
dotnet watch run

# Production mode
dotnet run --configuration Release
```

### Database Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Remove last migration (if not applied)
dotnet ef migrations remove
```

### Logging

Logs are written to:
- **Console** - Development mode
- **File** - `logs/sentinelLog-{Date}.log` (rolling daily)

Log levels:
- **Development**: `Information` and above
- **Production**: `Warning` and above

## 📝 Change Log

### Version 2.0.0 (December 10, 2025)

**What's New:**
- .NET 10.0 runtime
- Clean Architecture (Domain/Application/Infrastructure)
- Manual handler pattern (CQRS-style, MediatR alternative)
- FluentValidation integration with auto-registration
- Result Pattern for type-safe error handling
- Comprehensive test infrastructure
- 75% warning reduction (56 → 14)
- Generic, company-agnostic codebase
- Security sanitization (no sensitive data in repo)
- Example configurations and alert templates

**Breaking Changes:**
- Requires .NET 10.0 SDK
- Database migration required (EF Core 10.0)
- Configuration structure unchanged (backward compatible)

**Upgrade Path from 1.x:**
1. Install .NET 10.0 SDK
2. Backup your `appsettings.json` and database
3. Pull latest code
4. Restore configuration files
5. Run `dotnet ef database update`
6. Test all functionality

### Version 1.0.0 (Legacy)

- .NET 6.0 runtime (migrated to .NET 10.0 in v2.0.0)
- Monolithic Razor Pages architecture
- Direct service calls (no handler pattern)
- SQLite database
- Elasticsearch integration
- Quartz.NET scheduling
- Multi-channel notifications (Email, Teams)

## 📚 Resources

### Documentation

- **Example Files**: `docs/examples/` - Database init scripts, alert templates
- **Architecture**: Clean Architecture with handler pattern
- **Database**: Entity Framework Core with Code-First migrations

### External Resources

- [Elasticsearch Documentation](https://www.elastic.co/guide/index.html)
- [Quartz.NET Documentation](https://www.quartz-scheduler.net/documentation/)
- [Scriban Template Language](https://github.com/scriban/scriban)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Clean Architecture by Jason Taylor](https://jasontaylor.dev/clean-architecture-getting-started/)

### Support

For issues, questions, or contributions:
- **Issues**: [GitHub Issues](https://github.com/ChandreshRao/elastic-sentinel/issues)
- **Repository**: [elastic-sentinel](https://github.com/ChandreshRao/elastic-sentinel)

## 📄 License

(To be defined)

---

**Last Updated**: December 10, 2025  
**Version**: 2.0.0  
**Status**: Production Ready
