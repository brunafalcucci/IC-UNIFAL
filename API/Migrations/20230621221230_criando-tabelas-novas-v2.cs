using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class criandotabelasnovasv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirCompressor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    AirCompressorValue = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Use = table.Column<string>(type: "varchar", nullable: true),
                    Local = table.Column<string>(type: "varchar", nullable: true),
                    Moisture = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    Temperature = table.Column<string>(type: "varchar", nullable: true),
                    InletPressureControl = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_airCompressor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirConditioning",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    AirConditioningValue = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    Temperature = table.Column<string>(type: "varchar", nullable: true),
                    Acclimatized = table.Column<string>(type: "varchar", nullable: true),
                    Isolation = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_airConditioning", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boiler",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    BoilerValue = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Pressure = table.Column<string>(type: "varchar", nullable: true),
                    Condensed = table.Column<string>(type: "varchar", nullable: true),
                    Heat = table.Column<string>(type: "varchar", nullable: true),
                    Place = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    StudiesAndMeasures = table.Column<string>(type: "varchar", nullable: true),
                    Inspection = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_boiler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Condenser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    CondenserValue = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Ventilation = table.Column<string>(type: "varchar", nullable: true),
                    Isolation = table.Column<string>(type: "varchar", nullable: true),
                    Local = table.Column<string>(type: "varchar", nullable: true),
                    Heat = table.Column<string>(type: "varchar", nullable: true),
                    Condensed = table.Column<string>(type: "varchar", nullable: true),
                    Pressure = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_condenser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoolingSystem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    CoolingSystemValue = table.Column<string>(type: "varchar", nullable: true),
                    SystemOperationCooling = table.Column<string>(type: "varchar", nullable: true),
                    HeatTransferCooling = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Condenser = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    ChillerWaste = table.Column<string>(type: "varchar", nullable: true),
                    Air = table.Column<string>(type: "varchar", nullable: true),
                    Pressure = table.Column<string>(type: "varchar", nullable: true),
                    Refrigeration = table.Column<string>(type: "varchar", nullable: true),
                    Water = table.Column<string>(type: "varchar", nullable: true),
                    Temperature = table.Column<string>(type: "varchar", nullable: true),
                    Heat = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coolingSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Heating",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    HeatingValue = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Thermodynamics = table.Column<string>(type: "varchar", nullable: true),
                    Heat = table.Column<string>(type: "varchar", nullable: true),
                    Temperature = table.Column<string>(type: "varchar", nullable: true),
                    Fluid = table.Column<string>(type: "varchar", nullable: true),
                    AirType = table.Column<string>(type: "varchar", nullable: true),
                    Inspection = table.Column<string>(type: "varchar", nullable: true),
                    Isolation = table.Column<string>(type: "varchar", nullable: true),
                    Use = table.Column<string>(type: "varchar", nullable: true),
                    Place = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_heating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LightingSystem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    LightingSystemValue = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    ConstructionStructure = table.Column<string>(type: "varchar", nullable: true),
                    Operation = table.Column<string>(type: "varchar", nullable: true),
                    Reactor = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lightingSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    MotorValue = table.Column<string>(type: "varchar", nullable: true),
                    Temperature = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Moisture = table.Column<string>(type: "varchar", nullable: true),
                    Maintenance = table.Column<string>(type: "varchar", nullable: true),
                    Cleaning = table.Column<string>(type: "varchar", nullable: true),
                    Noise = table.Column<string>(type: "varchar", nullable: true),
                    Operation = table.Column<string>(type: "varchar", nullable: true),
                    Ventilation = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ventilation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "int8", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndustrialSector = table.Column<string>(type: "varchar", nullable: false),
                    IndustryName = table.Column<string>(type: "varchar", nullable: false),
                    VentilationValue = table.Column<string>(type: "varchar", nullable: true),
                    Management = table.Column<string>(type: "varchar", nullable: true),
                    Performance = table.Column<string>(type: "varchar", nullable: true),
                    Use = table.Column<string>(type: "varchar", nullable: true),
                    FanControl = table.Column<string>(type: "varchar", nullable: true),
                    Functionality = table.Column<string>(type: "varchar", nullable: true),
                    AirReduction = table.Column<string>(type: "varchar", nullable: true),
                    AirRecycling = table.Column<string>(type: "varchar", nullable: true),
                    Ultima_Atualizacao = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ventilation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirCompressor");

            migrationBuilder.DropTable(
                name: "AirConditioning");

            migrationBuilder.DropTable(
                name: "Boiler");

            migrationBuilder.DropTable(
                name: "Condenser");

            migrationBuilder.DropTable(
                name: "CoolingSystem");

            migrationBuilder.DropTable(
                name: "Heating");

            migrationBuilder.DropTable(
                name: "LightingSystem");

            migrationBuilder.DropTable(
                name: "Motor");

            migrationBuilder.DropTable(
                name: "Ventilation");
        }
    }
}
