START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220110164100_IncreasePasswordLength') THEN
        
        ALTER TABLE "Users" ALTER COLUMN "Password" TYPE character varying(60);
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220110164100_IncreasePasswordLength', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;