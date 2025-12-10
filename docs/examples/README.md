# Example Configuration Files

This folder contains example configuration files and templates to help you get started with ElasticSentinel.

## Database Setup

### `onetimescript.example.sql`
Example SQL script to populate initial configuration data. This script demonstrates:
- Elasticsearch cluster configuration
- Email connector setup (SMTP)
- Microsoft Teams webhook configuration
- Query sources and request mappings
- Response structure mappings
- Notification templates
- Alert scheduler configuration

**Usage:**
1. Copy this file to create your own version
2. Replace all placeholder values with your actual credentials
3. Run against your SQLite database after migrations

See [CONFIGURATION.md](../../CONFIGURATION.md) for detailed setup instructions.

## Alert Templates

### `error-alert-template.html`
Generic HTML template for error alert notifications. Demonstrates:
- Scriban template syntax
- Looping through error items
- Styled HTML email layout
- Customizable fields

**Template Variables:**
- `item.serverName`: Server identifier
- `item.errorLogDate`: Error occurrence date
- `item.timestamp`: Log timestamp
- `item.errorId`: Error code/ID
- `item.logMessage`: Error description

### `anomaly-alert-template.html`
Generic HTML template for anomaly/threshold detection alerts. Demonstrates:
- Aggregated metrics display
- Threshold breach notifications
- Pattern detection alerts

**Template Variables:**
- `item.responseCode`: Metric identifier
- `item.count`: Occurrence count or metric value

## Customization

All examples use:
- **Templating Engine**: Scriban (https://github.com/scriban/scriban)
- **Syntax**: `{{ variable }}` for output, `{{- for item in lst }}` for loops
- **Styling**: Inline CSS for email compatibility

Customize these templates by:
1. Modifying the HTML structure and styling
2. Adding/removing template variables
3. Adjusting the response mapping in your queries
4. Creating your own templates based on these examples

## Directory Structure

```
docs/examples/
├── README.md                       # This file
├── onetimescript.example.sql      # Database initialization script
├── error-alert-template.html      # Error notification template
└── anomaly-alert-template.html    # Anomaly detection template
```
