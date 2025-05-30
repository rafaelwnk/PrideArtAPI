using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrideArtAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFollows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_FollowedId",
                table: "UserFollows");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "UserFollows",
                newName: "FolloweeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_FolloweeId",
                table: "UserFollows",
                column: "FolloweeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_FolloweeId",
                table: "UserFollows");

            migrationBuilder.RenameColumn(
                name: "FolloweeId",
                table: "UserFollows",
                newName: "FollowedId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_FollowedId",
                table: "UserFollows",
                column: "FollowedId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
