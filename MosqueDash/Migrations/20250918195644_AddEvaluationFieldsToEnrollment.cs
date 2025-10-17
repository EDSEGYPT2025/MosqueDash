using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MosqueDash.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluationFieldsToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinalEvaluation",
                table: "Enrollments",
                newName: "EvaluationNotes");

            migrationBuilder.AddColumn<string>(
                name: "TeacherEvaluation",
                table: "Enrollments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeacherEvaluation",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "EvaluationNotes",
                table: "Enrollments",
                newName: "FinalEvaluation");
        }
    }
}
