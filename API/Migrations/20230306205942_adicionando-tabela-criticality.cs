using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class adicionandotabelacriticality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CriticalityIndex",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    CriticalityIndexValue = table.Column<string>(type: "varchar", nullable: true),
                    CostsManagement = table.Column<string>(type: "varchar", nullable: true),
                    IndustrialManagement = table.Column<string>(type: "varchar", nullable: true),
                    EnvironmentalQuality = table.Column<string>(type: "varchar", nullable: true),
                    Investiments = table.Column<string>(type: "varchar", nullable: true),
                    ElectricityExpensives = table.Column<string>(type: "varchar", nullable: true),
                    Predictive = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    Preventive = table.Column<string>(type: "varchar", nullable: true),
                    Corrective = table.Column<string>(type: "varchar", nullable: true),
                    Governance = table.Column<string>(type: "varchar", nullable: true),
                    EnvironmentalRisks = table.Column<string>(type: "varchar", nullable: true),
                    EnergyUse = table.Column<string>(type: "varchar", nullable: true),
                    EnergyGeneration = table.Column<string>(type: "varchar", nullable: true),
                    Technology = table.Column<string>(type: "varchar", nullable: true),
                    Process = table.Column<string>(type: "varchar", nullable: true),
                    TechnologyDataCollection = table.Column<string>(type: "varchar", nullable: true),
                    Instrumentation = table.Column<string>(type: "varchar", nullable: true),
                    PredictiveOperation = table.Column<string>(type: "varchar", nullable: true),
                    Operation_Work = table.Column<string>(type: "varchar", nullable: true),
                    AvailabilityOfRepairPersonnel = table.Column<string>(type: "varchar", nullable: true),
                    OperationSensitivity = table.Column<string>(type: "varchar", nullable: true),
                    MeanTimeBetweenFailures = table.Column<string>(type: "varchar", nullable: true),
                    WorkLoad = table.Column<string>(type: "varchar", nullable: true),
                    Activities = table.Column<string>(type: "varchar", nullable: true),
                    AverageRepairTime = table.Column<string>(type: "varchar", nullable: true),
                    AvailabilityOfRequiredParts = table.Column<string>(type: "varchar", nullable: true),
                    SkillLevels = table.Column<string>(type: "varchar", nullable: true),
                    ManagementStrategy = table.Column<string>(type: "varchar", nullable: true),
                    Toxicity = table.Column<string>(type: "varchar", nullable: true),
                    Solubility = table.Column<string>(type: "varchar", nullable: true),
                    No_Renewable = table.Column<string>(type: "varchar", nullable: true),
                    Renewable = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_criticalityIndex", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriticalityIndex");
        }
    }
}
