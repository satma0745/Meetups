START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220206172635_UpdateUserDiscriminator') THEN
        
        ALTER TABLE users RENAME COLUMN account_type TO role;

        UPDATE users
        SET role = 'Guest'
        WHERE role = 'guest';
        
        UPDATE users
        SET role = 'Organizer'
        WHERE role = 'organizer';
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220206172635_UpdateUserDiscriminator', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;