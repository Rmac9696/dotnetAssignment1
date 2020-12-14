using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DOTNET_lab4.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisement",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Budget = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Adverts",
                columns: table => new
                {
                    AdvertID = table.Column<int>(nullable: false),
                    CommunityID = table.Column<string>(nullable: false),
                    AdvertisementID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adverts", x => new { x.AdvertID, x.CommunityID });
                    table.ForeignKey(
                        name: "FK_Adverts_Advertisement_AdvertisementID",
                        column: x => x.AdvertisementID,
                        principalTable: "Advertisement",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Adverts_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    StudentID = table.Column<int>(nullable: false),
                    CommunityID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => new { x.StudentID, x.CommunityID });
                    table.ForeignKey(
                        name: "FK_Memberships_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_AdvertisementID",
                table: "Adverts",
                column: "AdvertisementID");

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_CommunityID",
                table: "Adverts",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_CommunityID",
                table: "Memberships",
                column: "CommunityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adverts");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "Community");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
