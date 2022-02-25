START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205200800_AddOrganizers') THEN
        
        DELETE FROM meetups;
        ALTER TABLE meetups ADD organizer_id uuid NOT NULL;
        
        CREATE INDEX "IX_meetups_organizer_id" ON meetups (organizer_id);
        ALTER TABLE meetups
            ADD CONSTRAINT fk_meetups_organizers_organizer_id
                FOREIGN KEY (organizer_id)
                REFERENCES users (id)
                ON DELETE CASCADE;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220205200800_AddOrganizers', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;