START TRANSACTION;

DO $EF$
BEGIN
    CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
        "MigrationId" character varying(150) NOT NULL,
        "ProductVersion" character varying(32) NOT NULL,
        CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
    );
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220109161533_AddUsersAndMeetups') THEN
        
        CREATE TABLE "Meetups" (
            "Id" uuid NOT NULL,
            "Topic" character varying(100) NOT NULL,
            "Place" character varying(75) NOT NULL,
            "Duration" interval NOT NULL,
            "StartTime" timestamp with time zone NOT NULL,
            
            CONSTRAINT "PK_Meetups" PRIMARY KEY ("Id")
        );
        CREATE UNIQUE INDEX "IX_Meetups_Topic" ON "Meetups" ("Topic");
    
        CREATE TABLE "Users" (
            "Id" uuid NOT NULL,
            "Username" character varying(30) NOT NULL,
            "Password" character varying(30) NOT NULL,
            "DisplayName" character varying(45) NOT NULL,
            
            CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
        );
        CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220109161533_AddUsersAndMeetups', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;

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

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220114134252_AddMeetupSignUps') THEN
        
        CREATE TABLE "MeetupUser" (
            "MeetupsSignedUpToId" uuid NOT NULL,
            "SignedUpUsersId" uuid NOT NULL,
            
            CONSTRAINT "PK_MeetupUser" PRIMARY KEY ("MeetupsSignedUpToId", "SignedUpUsersId"),
            
            CONSTRAINT "FK_MeetupUser_Meetups_MeetupsSignedUpToId"
                FOREIGN KEY ("MeetupsSignedUpToId")
                REFERENCES "Meetups" ("Id")
                ON DELETE CASCADE,
            CONSTRAINT "FK_MeetupUser_Users_SignedUpUsersId"
                FOREIGN KEY ("SignedUpUsersId")
                REFERENCES "Users" ("Id")
                ON DELETE CASCADE
        );
        CREATE INDEX "IX_MeetupUser_SignedUpUsersId" ON "MeetupUser" ("SignedUpUsersId");
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220114134252_AddMeetupSignUps', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
        
        ALTER TABLE "MeetupUser"
            DROP CONSTRAINT "PK_MeetupUser",
            DROP CONSTRAINT "FK_MeetupUser_Meetups_MeetupsSignedUpToId",
            DROP CONSTRAINT "FK_MeetupUser_Users_SignedUpUsersId";
        ALTER TABLE "MeetupUser" RENAME TO meetups_users_signup;
        ALTER TABLE meetups_users_signup RENAME COLUMN "MeetupsSignedUpToId" TO meetup_id;
        ALTER TABLE meetups_users_signup RENAME COLUMN "SignedUpUsersId" TO signed_up_user_id;
        ALTER INDEX "IX_MeetupUser_SignedUpUsersId" RENAME TO ix_meetups_users_signup_signed_up_user_id;
        ALTER TABLE meetups_users_signup ADD CONSTRAINT pk_meetups_users_signup PRIMARY KEY (meetup_id, signed_up_user_id);
        
        ALTER TABLE "Users" DROP CONSTRAINT "PK_Users";
        ALTER TABLE "Users" RENAME TO users;
        ALTER TABLE users RENAME COLUMN "Id" TO id;
        ALTER TABLE users RENAME COLUMN "Username" TO username;
        ALTER TABLE users RENAME COLUMN "Password" TO password;
        ALTER TABLE users RENAME COLUMN "DisplayName" TO display_name;
        ALTER INDEX "IX_Users_Username" RENAME TO ux_users_username;
        ALTER TABLE users ADD CONSTRAINT pk_users PRIMARY KEY (id);
        
        ALTER TABLE "Meetups" DROP CONSTRAINT "PK_Meetups";
        ALTER TABLE "Meetups" RENAME TO meetups;
        ALTER TABLE meetups RENAME COLUMN "Id" TO id;
        ALTER TABLE meetups RENAME COLUMN "Topic" TO topic;
        ALTER TABLE meetups RENAME COLUMN "Place" TO place;
        ALTER TABLE meetups RENAME COLUMN "Duration_Minutes" TO duration_minutes;
        ALTER TABLE meetups RENAME COLUMN "Duration_Hours" TO duration_hours;
        ALTER TABLE meetups RENAME COLUMN "StartTime" TO start_time;
        ALTER INDEX "IX_Meetups_Topic" RENAME TO ux_meetups_topic;
        ALTER TABLE meetups ADD CONSTRAINT pk_meetups PRIMARY KEY (id);
        
        ALTER TABLE "RefreshTokens" DROP CONSTRAINT "PK_RefreshTokens";
        ALTER TABLE "RefreshTokens" RENAME TO refresh_tokens;
        ALTER TABLE refresh_tokens RENAME COLUMN "TokenId" TO token_id;
        ALTER TABLE refresh_tokens RENAME COLUMN "UserId" TO user_id;
        ALTER TABLE refresh_tokens ADD CONSTRAINT pk_refresh_tokens PRIMARY KEY (token_id);
        CREATE INDEX ix_refresh_tokens_user_id ON refresh_tokens (user_id);
        
        ALTER TABLE meetups_users_signup
            ADD CONSTRAINT fk_meetups_users_signup_users_signed_up_user_id
                FOREIGN KEY (signed_up_user_id)
                REFERENCES users (id)
                ON DELETE CASCADE,
            ADD CONSTRAINT fk_meetups_users_signup_meetups_signed_up_user_id
                FOREIGN KEY (meetup_id)
                REFERENCES meetups (id)
                ON DELETE CASCADE;
        
        ALTER TABLE refresh_tokens
            ADD CONSTRAINT fk_users_refresh_tokens_user_id
                FOREIGN KEY (user_id)
                REFERENCES users (id)
                ON DELETE CASCADE;
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220116100522_FollowNamingConventions', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
        
        ALTER TABLE meetups_users_signup
            DROP CONSTRAINT fk_meetups_users_signup_meetups_signed_up_user_id,
            DROP CONSTRAINT fk_meetups_users_signup_users_signed_up_user_id,
            DROP CONSTRAINT pk_meetups_users_signup;
        
        ALTER TABLE meetups_users_signup RENAME TO meetups_guests_signup;
        ALTER TABLE meetups_guests_signup RENAME COLUMN signed_up_user_id TO signed_up_guest_id;
        ALTER INDEX ix_meetups_users_signup_signed_up_user_id RENAME TO ix_meetups_guests_signup_signed_up_guest_id;
        
        ALTER TABLE meetups_guests_signup
            ADD CONSTRAINT pk_meetups_guests_signup
                PRIMARY KEY (meetup_id, signed_up_guest_id);
        ALTER TABLE meetups_guests_signup
            ADD CONSTRAINT fk_meetups_guests_signup_guests_signed_up_guest_id
                FOREIGN KEY (signed_up_guest_id)
                    REFERENCES users (id)
                    ON DELETE CASCADE,
            ADD CONSTRAINT fk_meetups_guests_signup_meetups_meetup_id
                FOREIGN KEY (meetup_id)
                    REFERENCES meetups (id)
                    ON DELETE CASCADE;
        
        ALTER TABLE users
            ADD account_type text NOT NULL DEFAULT 'guest';        
        
        INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
        VALUES ('20220205183337_AddGuests', '6.0.1');
        
    END IF;
END $EF$;

COMMIT;

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