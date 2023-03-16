using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApi.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            //Triggers for Player
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnUpdate
                    AFTER UPDATE ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPlayerTimestampOnInsert
                    AFTER INSERT ON Players
                    BEGIN
                        UPDATE Players
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");

            //Triggers for Team
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTeamTimestampOnUpdate
                    AFTER UPDATE ON Teams
                    BEGIN
                        UPDATE Teams
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTeamTimestampOnInsert
                    AFTER INSERT ON Teams
                    BEGIN
                        UPDATE Teams
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");

            //Triggers for League
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetLeagueTimestampOnUpdate
                    AFTER UPDATE ON Leagues
                    BEGIN
                        UPDATE Leagues
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetLeagueTimestampOnInsert
                    AFTER INSERT ON Leagues
                    BEGIN
                        UPDATE Leagues
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
        }
    }
}
