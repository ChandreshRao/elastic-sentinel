-- Example Database Initialization Script
-- This script provides sample data structure for Elastic Sentinel
-- Replace all placeholder values with your actual configuration

-- ==========================================
-- ELASTICSEARCH CONFIGURATION
-- ==========================================
INSERT INTO elastic_configuration
(
	cluster_name,
	host,
	[user_name],
	[password],
	certificate_thumbprint,
	is_enabled
)
SELECT 'Your-Elasticsearch-Cluster' cluster_name,
		'https://your-elasticsearch-host:9200' host,
		'your-username' [user_name],
		'your-password' [password],
		'YOUR-CERTIFICATE-THUMBPRINT-IF-NEEDED' certificate_thumbprint,
		1 is_enabled
WHERE NOT EXISTS (SELECT 1 FROM elastic_configuration WHERE cluster_name = 'Your-Elasticsearch-Cluster');

-- ==========================================
-- EMAIL CONNECTOR CONFIGURATION
-- ==========================================
INSERT INTO email_connector
(
	email_connector_name,
	from_email,
	smtp_server,
	alternate_smtp_server,
	smtp_port,
	[user_name],
	[password],
	is_enabled
)
SELECT 'Primary-Email-Connector' email_connector_name,
	'alerts@yourdomain.com' from_email,
	'smtp.yourdomain.com' smtp_server,
	'smtp-backup.yourdomain.com' alternate_smtp_server,
	587 smtp_port,
	'smtp-username' [user_name],
	'smtp-password' [password],
	1 is_enabled
WHERE NOT EXISTS (SELECT 1 FROM email_connector WHERE email_connector_name = 'Primary-Email-Connector');

-- ==========================================
-- EMAIL RECIPIENTS CONFIGURATION
-- ==========================================
INSERT INTO email_connector_detail
(
	email_connector_detail_name,
	email_subject,
	to_emails,
	cc_emails
)
SELECT 'Alert-Recipients' email_connector_detail_name,
	'Elasticsearch Alert' email_subject,
	'admin@yourdomain.com,team@yourdomain.com' to_emails,
	'manager@yourdomain.com' cc_emails
WHERE NOT EXISTS (SELECT 1 FROM email_connector_detail WHERE email_connector_detail_name = 'Alert-Recipients');

-- ==========================================
-- MICROSOFT TEAMS WEBHOOK CONFIGURATION
-- ==========================================
INSERT INTO teams_connector
(
	teams_connector_name,
	webhook_url,
	is_enabled
)
SELECT 'Teams-Alert-Connector' teams_connector_name,
	'https://your-org.webhook.office.com/webhookb2/YOUR-WEBHOOK-ID' webhook_url,
	1 is_enabled
WHERE NOT EXISTS (SELECT 1 FROM teams_connector WHERE teams_connector_name = 'Teams-Alert-Connector');

-- ==========================================
-- EXAMPLE QUERY SOURCE (ELASTIC DSL)
-- ==========================================
INSERT INTO elastic_dynamic_query_source
(
	source_name,
	source_query,
	source_type
)
SELECT 'ErrorLog-Query-Example' source_name,
	'{"query":{"bool":{"must":[{"match":{"level":{"query":"ERROR"}}},{"range":{"@timestamp":{"gte":"now-1m/m","lte":"now"}}}]}}}' source_query,
	'application/json' source_type
WHERE NOT EXISTS (SELECT 1 FROM elastic_dynamic_query_source WHERE source_name = 'ErrorLog-Query-Example');

-- ==========================================
-- EXAMPLE QUERY REQUEST CONFIGURATION
-- ==========================================
WITH tmp_query_source AS
(
	SELECT elastic_dynamic_query_source_id FROM elastic_dynamic_query_source WHERE source_name = 'ErrorLog-Query-Example'
)
INSERT INTO elastic_dynamic_query_request_detail
(
	request_name,
	http_method,
	index_name,
	is_index_expression,
	query_type,
	query_params,
	elastic_dynamic_query_source_id,
	headers,
	is_enabled
)
SELECT 'Error-Log-Search' request_name,
	'POST' http_method,
	'your-index-pattern-*' index_name,
	1 is_index_expression,
	'_search' query_type,
	null query_params,
	tmp_query_source.elastic_dynamic_query_source_id,
	null headers,
	1 is_enabled
FROM tmp_query_source
WHERE NOT EXISTS (SELECT 1 FROM elastic_dynamic_query_request_detail WHERE request_name = 'Error-Log-Search');

-- ==========================================
-- EXAMPLE QUERY RESPONSE CONFIGURATION
-- ==========================================
WITH tmp_query AS
(
	SELECT elastic_query_id FROM elastic_query WHERE query_name = 'Example-Error-Alert-Query'
)
INSERT INTO elastic_dynamic_query_response_detail
(
	response_name,
	elastic_query_id,
	index_name_response_key,
	aggregation_response_key
)
SELECT 'Error-Response-Mapping' response_name,
	tmp_query.elastic_query_id,
	'hits.hits' index_name_response_key,
	null aggregation_response_key
FROM tmp_query
WHERE NOT EXISTS (SELECT 1 FROM elastic_dynamic_query_response_detail WHERE response_name = 'Error-Response-Mapping');

-- ==========================================
-- EXAMPLE NOTIFICATION TEMPLATE
-- ==========================================
INSERT INTO notification_template
(
	template_name,
	template_title,
	template_body
)
SELECT 'Error-Alert-Template' template_name,
	'Elasticsearch Error Alert' template_title,
	'<h2>Error Alert</h2><p>{{ count }} errors detected in the last {{ timeframe }}</p>' template_body
WHERE NOT EXISTS (SELECT 1 FROM notification_template WHERE template_name = 'Error-Alert-Template');

-- ==========================================
-- EXAMPLE ALERT SCHEDULER
-- ==========================================
WITH tmp_query AS
(
	SELECT elastic_query_id FROM elastic_query WHERE query_name = 'Example-Error-Alert-Query'
),
tmp_connector AS
(
	SELECT email_connector_id FROM email_connector WHERE email_connector_name = 'Primary-Email-Connector'
),
tmp_connector_detail AS
(
	SELECT email_connector_detail_id FROM email_connector_detail WHERE email_connector_detail_name = 'Alert-Recipients'
),
tmp_template AS
(
	SELECT notification_template_id FROM notification_template WHERE template_name = 'Error-Alert-Template'
)
INSERT INTO alert_scheduler_config
(
	scheduler_name,
	scheduler_description,
	cron_expression,
	elastic_query_id,
	email_connector_id,
	email_connector_detail_id,
	notification_template_id,
	is_enabled
)
SELECT 'Hourly-Error-Check' scheduler_name,
	'Check for errors every hour' scheduler_description,
	'0 0 * * * ?' cron_expression,  -- Every hour
	tmp_query.elastic_query_id,
	tmp_connector.email_connector_id,
	tmp_connector_detail.email_connector_detail_id,
	tmp_template.notification_template_id,
	1 is_enabled
FROM tmp_query, tmp_connector, tmp_connector_detail, tmp_template
WHERE NOT EXISTS (SELECT 1 FROM alert_scheduler_config WHERE scheduler_name = 'Hourly-Error-Check');

-- ==========================================
-- NOTES
-- ==========================================
-- 1. Replace all 'your-*' placeholders with actual values
-- 2. Update SMTP settings according to your email provider
-- 3. Create Teams webhook following Microsoft documentation
-- 4. Customize Elasticsearch queries for your indices
-- 5. Adjust cron expressions for your alerting schedule
-- 6. Test with small time windows before production deployment
