START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
        
        CREATE TABLE cities (
            id uuid NOT NULL,
            name character varying(30) NOT NULL,
            CONSTRAINT pk_cities PRIMARY KEY (id)
        );
        CREATE UNIQUE INDEX ux_cities_name ON cities (name);
        
        INSERT INTO cities(id, name)
        VALUES ('0863abb7-b3fc-4ae8-ab91-81ae9780a14e', 'Oslo');
        
        ALTER TABLE meetups RENAME COLUMN place TO place_address;
        ALTER TABLE meetups ADD place_city_id uuid NOT NULL DEFAULT '0863abb7-b3fc-4ae8-ab91-81ae9780a14e';
        CREATE INDEX ix_meetups_place_city_id ON meetups (place_city_id);
        ALTER TABLE meetups
            ADD CONSTRAINT fk_meetups_cities_place_city_id
                FOREIGN KEY (place_city_id)
                REFERENCES cities (id)
                ON DELETE CASCADE;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220222124412_AddCities', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;