﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NegotiationApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
