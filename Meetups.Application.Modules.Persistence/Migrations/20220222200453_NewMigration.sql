START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222200453_NewMigration') THEN
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220222200453_NewMigration', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;