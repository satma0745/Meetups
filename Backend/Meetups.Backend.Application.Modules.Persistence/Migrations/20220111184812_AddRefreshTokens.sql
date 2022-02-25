START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220111184812_AddRefreshTokens') THEN
        
        CREATE TABLE "RefreshTokens" (
            "TokenId" uuid NOT NULL,
            "UserId" uuid NOT NULL,
            
            CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("TokenId")
        );
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220111184812_AddRefreshTokens', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;