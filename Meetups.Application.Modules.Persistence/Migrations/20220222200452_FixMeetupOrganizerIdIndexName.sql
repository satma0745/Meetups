START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222200452_FixMeetupOrganizerIdIndexName') THEN
        
        ALTER INDEX "IX_meetups_organizer_id" RENAME TO ix_meetups_organizer_id;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220222200452_FixMeetupOrganizerIdIndexName', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;