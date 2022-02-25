START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220207173657_AddMeetupDurationConversion') THEN
        
        ALTER TABLE meetups ADD duration integer NOT NULL DEFAULT 0;
        
        UPDATE meetups
        SET duration = duration_hours * 60 + duration_minutes;
        
        ALTER TABLE meetups
            DROP COLUMN duration_hours,
            DROP COLUMN duration_minutes;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220207173657_AddMeetupDurationConversion', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;