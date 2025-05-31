using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrideArtAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserFollowsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_UserId",
                table: "UserFollows");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserFollows",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_UserId",
                table: "UserFollows",
                newName: "IX_UserFollows_FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_FollowerId",
                table: "UserFollows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "UserFollows",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                newName: "IX_UserFollows_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_UserId",
                table: "UserFollows",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
