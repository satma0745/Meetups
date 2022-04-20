START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220112191357_UseOwnedEntityForMeetupDuration') THEN
        
        ALTER TABLE "Meetups"
            DROP COLUMN "Duration",
            ADD "Duration_Hours" integer NOT NULL DEFAULT 0,
            ADD "Duration_Minutes" integer NOT NULL DEFAULT 0;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220112191357_UseOwnedEntityForMeetupDuration', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;