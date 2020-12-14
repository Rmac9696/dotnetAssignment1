using Microsoft.EntityFrameworkCore.Migrations;

namespace DOTNET_lab4.Migrations
{
    public partial class newMig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Advertisement_AdvertisementID",
                table: "Adverts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adverts",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_AdvertisementID",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "AdvertID",
                table: "Adverts");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementID",
                table: "Adverts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adverts",
                table: "Adverts",
                columns: new[] { "AdvertisementID", "CommunityID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Advertisement_AdvertisementID",
                table: "Adverts",
                column: "AdvertisementID",
                principalTable: "Advertisement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Advertisement_AdvertisementID",
                table: "Adverts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adverts",
                table: "Adverts");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementID",
                table: "Adverts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AdvertID",
                table: "Adverts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adverts",
                table: "Adverts",
                columns: new[] { "AdvertID", "CommunityID" });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_AdvertisementID",
                table: "Adverts",
                column: "AdvertisementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Advertisement_AdvertisementID",
                table: "Adverts",
                column: "AdvertisementID",
                principalTable: "Advertisement",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
