using ElasticSentinel.Infrastructure.Persistence;
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElasticSentinel.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "elastic_configuration",
                columns: table => new
                {
                    elastic_configuration_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cluster_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    host = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    user_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    certificate_thumbprint = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_configuration", x => x.elastic_configuration_id);
                });

            migrationBuilder.CreateTable(
                name: "elastic_dynamic_query_response_detail",
                columns: table => new
                {
                    elastic_dynamic_query_response_detail_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    query_response_mapper_name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_dynamic_query_response_detail", x => x.elastic_dynamic_query_response_detail_id);
                });

            migrationBuilder.CreateTable(
                name: "elastic_dynamic_query_source",
                columns: table => new
                {
                    elastic_dynamic_query_source_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    source_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    source_query = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    source_type = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_dynamic_query_source", x => x.elastic_dynamic_query_source_id);
                });

            migrationBuilder.CreateTable(
                name: "email_connector",
                columns: table => new
                {
                    email_connector_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email_connector_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    from_email = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    smtp_server = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    alternate_smtp_server = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    smtp_port = table.Column<int>(type: "INTEGER", nullable: false),
                    user_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_connector", x => x.email_connector_id);
                });

            migrationBuilder.CreateTable(
                name: "email_connector_detail",
                columns: table => new
                {
                    email_connector_detail_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email_connector_detail_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    email_subject = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    to_emails = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    cc_emails = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_connector_detail", x => x.email_connector_detail_id);
                });

            migrationBuilder.CreateTable(
                name: "notification_template",
                columns: table => new
                {
                    notification_template_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    template_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    template_content = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_template", x => x.notification_template_id);
                });

            migrationBuilder.CreateTable(
                name: "teams_connector",
                columns: table => new
                {
                    teams_connector_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    teams_connector_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    webhook_url = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams_connector", x => x.teams_connector_id);
                });

            migrationBuilder.CreateTable(
                name: "elastic_dynamic_query_response_structure",
                columns: table => new
                {
                    elastic_dynamic_query_response_structure_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    elastic_dynamic_query_response_detail_id = table.Column<short>(type: "INTEGER", nullable: false),
                    is_index_field_array = table.Column<bool>(type: "INTEGER", nullable: false),
                    index_root_field_name = table.Column<string>(type: "TEXT", nullable: true),
                    index_field_name = table.Column<string>(type: "TEXT", nullable: false),
                    alias_field_name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_dynamic_query_response_structure", x => x.elastic_dynamic_query_response_structure_id);
                    table.ForeignKey(
                        name: "FK_elastic_dynamic_query_response_structure_elastic_dynamic_query_response_detail_elastic_dynamic_query_response_detail_id",
                        column: x => x.elastic_dynamic_query_response_detail_id,
                        principalTable: "elastic_dynamic_query_response_detail",
                        principalColumn: "elastic_dynamic_query_response_detail_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "elastic_dynamic_query_request_detail",
                columns: table => new
                {
                    elastic_dynamic_query_request_detail_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    request_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    http_method = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    index_name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    is_index_expression = table.Column<bool>(type: "INTEGER", nullable: false),
                    query_type = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    query_params = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    elastic_dynamic_query_source_id = table.Column<short>(type: "INTEGER", nullable: false),
                    headers = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    auth_type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    message_body = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_dynamic_query_request_detail", x => x.elastic_dynamic_query_request_detail_id);
                    table.ForeignKey(
                        name: "FK_elastic_dynamic_query_request_detail_elastic_dynamic_query_source_elastic_dynamic_query_source_id",
                        column: x => x.elastic_dynamic_query_source_id,
                        principalTable: "elastic_dynamic_query_source",
                        principalColumn: "elastic_dynamic_query_source_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "elastic_query",
                columns: table => new
                {
                    elastic_query_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    query_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    query_description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    is_dynamic = table.Column<bool>(type: "INTEGER", nullable: false),
                    elastic_dynamic_query_request_detail_id = table.Column<short>(type: "INTEGER", nullable: false),
                    elastic_dynamic_query_response_detail_id = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_elastic_query", x => x.elastic_query_id);
                    table.ForeignKey(
                        name: "FK_elastic_query_elastic_dynamic_query_request_detail_elastic_dynamic_query_request_detail_id",
                        column: x => x.elastic_dynamic_query_request_detail_id,
                        principalTable: "elastic_dynamic_query_request_detail",
                        principalColumn: "elastic_dynamic_query_request_detail_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_elastic_query_elastic_dynamic_query_response_detail_elastic_dynamic_query_response_detail_id",
                        column: x => x.elastic_dynamic_query_response_detail_id,
                        principalTable: "elastic_dynamic_query_response_detail",
                        principalColumn: "elastic_dynamic_query_response_detail_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alert_scheduler_config",
                columns: table => new
                {
                    alert_scheduler_config_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    elastic_configuration_id = table.Column<short>(type: "INTEGER", nullable: false),
                    scheduler_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    scheduler_group = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    cron_expression = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    elastic_query_id = table.Column<short>(type: "INTEGER", nullable: false),
                    email_connector_id = table.Column<short>(type: "INTEGER", nullable: false),
                    teams_connector_id = table.Column<short>(type: "INTEGER", nullable: false),
                    email_connector_detail_id = table.Column<short>(type: "INTEGER", nullable: false),
                    notification_template_id = table.Column<short>(type: "INTEGER", nullable: false),
                    is_enabled = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_scheduler_config", x => x.alert_scheduler_config_id);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_elastic_configuration_elastic_configuration_id",
                        column: x => x.elastic_configuration_id,
                        principalTable: "elastic_configuration",
                        principalColumn: "elastic_configuration_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_elastic_query_elastic_query_id",
                        column: x => x.elastic_query_id,
                        principalTable: "elastic_query",
                        principalColumn: "elastic_query_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_email_connector_detail_email_connector_detail_id",
                        column: x => x.email_connector_detail_id,
                        principalTable: "email_connector_detail",
                        principalColumn: "email_connector_detail_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_email_connector_email_connector_id",
                        column: x => x.email_connector_id,
                        principalTable: "email_connector",
                        principalColumn: "email_connector_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_notification_template_notification_template_id",
                        column: x => x.notification_template_id,
                        principalTable: "notification_template",
                        principalColumn: "notification_template_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_config_teams_connector_teams_connector_id",
                        column: x => x.teams_connector_id,
                        principalTable: "teams_connector",
                        principalColumn: "teams_connector_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alert_scheduler_detail",
                columns: table => new
                {
                    alert_scheduler_detail_id = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alert_scheduler_config_id = table.Column<short>(type: "INTEGER", nullable: false),
                    query_filter_dttm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_run_dttm = table.Column<DateTime>(type: "TEXT", nullable: true),
                    last_run_status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alert_scheduler_detail", x => x.alert_scheduler_detail_id);
                    table.ForeignKey(
                        name: "FK_alert_scheduler_detail_alert_scheduler_config_alert_scheduler_config_id",
                        column: x => x.alert_scheduler_config_id,
                        principalTable: "alert_scheduler_config",
                        principalColumn: "alert_scheduler_config_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_elastic_configuration_id",
                table: "alert_scheduler_config",
                column: "elastic_configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_elastic_query_id",
                table: "alert_scheduler_config",
                column: "elastic_query_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_email_connector_detail_id",
                table: "alert_scheduler_config",
                column: "email_connector_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_email_connector_id",
                table: "alert_scheduler_config",
                column: "email_connector_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_notification_template_id",
                table: "alert_scheduler_config",
                column: "notification_template_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_config_teams_connector_id",
                table: "alert_scheduler_config",
                column: "teams_connector_id");

            migrationBuilder.CreateIndex(
                name: "IX_alert_scheduler_detail_alert_scheduler_config_id",
                table: "alert_scheduler_detail",
                column: "alert_scheduler_config_id");

            migrationBuilder.CreateIndex(
                name: "IX_elastic_dynamic_query_request_detail_elastic_dynamic_query_source_id",
                table: "elastic_dynamic_query_request_detail",
                column: "elastic_dynamic_query_source_id");

            migrationBuilder.CreateIndex(
                name: "IX_elastic_dynamic_query_response_structure_elastic_dynamic_query_response_detail_id",
                table: "elastic_dynamic_query_response_structure",
                column: "elastic_dynamic_query_response_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_elastic_query_elastic_dynamic_query_request_detail_id",
                table: "elastic_query",
                column: "elastic_dynamic_query_request_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_elastic_query_elastic_dynamic_query_response_detail_id",
                table: "elastic_query",
                column: "elastic_dynamic_query_response_detail_id");

            // Note: Initial data should be loaded manually using onetimescript.example.sql
            // See CONFIGURATION.md for setup instructions
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alert_scheduler_detail");

            migrationBuilder.DropTable(
                name: "elastic_dynamic_query_response_structure");

            migrationBuilder.DropTable(
                name: "alert_scheduler_config");

            migrationBuilder.DropTable(
                name: "elastic_configuration");

            migrationBuilder.DropTable(
                name: "elastic_query");

            migrationBuilder.DropTable(
                name: "email_connector_detail");

            migrationBuilder.DropTable(
                name: "email_connector");

            migrationBuilder.DropTable(
                name: "notification_template");

            migrationBuilder.DropTable(
                name: "teams_connector");

            migrationBuilder.DropTable(
                name: "elastic_dynamic_query_request_detail");

            migrationBuilder.DropTable(
                name: "elastic_dynamic_query_response_detail");

            migrationBuilder.DropTable(
                name: "elastic_dynamic_query_source");
        }
    }
}
