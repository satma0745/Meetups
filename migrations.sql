CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220109161533_AddUsersAndMeetups') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Username" character varying(30) NOT NULL,
        "Password" character varying(30) NOT NULL,
        "DisplayName" character varying(45) NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220109161533_AddUsersAndMeetups') THEN
    CREATE UNIQUE INDEX "IX_Meetups_Topic" ON "Meetups" ("Topic");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220109161533_AddUsersAndMeetups') THEN
    CREATE UNIQUE INDEX "IX_Users_Username" ON "Users" ("Username");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220109161533_AddUsersAndMeetups') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220110164100_IncreasePasswordLength') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220111184812_AddRefreshTokens') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220111184812_AddRefreshTokens', '6.0.1');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220112191357_UseOwnedEntityForMeetupDuration') THEN
    ALTER TABLE "Meetups" DROP COLUMN "Duration";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220112191357_UseOwnedEntityForMeetupDuration') THEN
    ALTER TABLE "Meetups" ADD "Duration_Hours" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220112191357_UseOwnedEntityForMeetupDuration') THEN
    ALTER TABLE "Meetups" ADD "Duration_Minutes" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220112191357_UseOwnedEntityForMeetupDuration') THEN
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
        CONSTRAINT "FK_MeetupUser_Meetups_MeetupsSignedUpToId" FOREIGN KEY ("MeetupsSignedUpToId") REFERENCES "Meetups" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_MeetupUser_Users_SignedUpUsersId" FOREIGN KEY ("SignedUpUsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220114134252_AddMeetupSignUps') THEN
    CREATE INDEX "IX_MeetupUser_SignedUpUsersId" ON "MeetupUser" ("SignedUpUsersId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220114134252_AddMeetupSignUps') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220114134252_AddMeetupSignUps', '6.0.1');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "MeetupUser" DROP CONSTRAINT "PK_MeetupUser";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "MeetupUser" DROP CONSTRAINT "FK_MeetupUser_Meetups_MeetupsSignedUpToId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "MeetupUser" DROP CONSTRAINT "FK_MeetupUser_Users_SignedUpUsersId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "Users" DROP CONSTRAINT "PK_Users";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "Meetups" DROP CONSTRAINT "PK_Meetups";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "RefreshTokens" DROP CONSTRAINT "PK_RefreshTokens";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "MeetupUser" RENAME TO meetups_users_signup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "Users" RENAME TO users;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "Meetups" RENAME TO meetups;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE "RefreshTokens" RENAME TO refresh_tokens;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups_users_signup RENAME COLUMN "MeetupsSignedUpToId" TO meetup_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups_users_signup RENAME COLUMN "SignedUpUsersId" TO signed_up_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER INDEX "IX_MeetupUser_SignedUpUsersId" RENAME TO ix_meetups_users_signup_signed_up_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE users RENAME COLUMN "Username" TO username;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE users RENAME COLUMN "Password" TO password;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE users RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE users RENAME COLUMN "DisplayName" TO display_name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER INDEX "IX_Users_Username" RENAME TO ux_users_username;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "Topic" TO topic;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "Place" TO place;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "Duration_Minutes" TO duration_minutes;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "Duration_Hours" TO duration_hours;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "Id" TO id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups RENAME COLUMN "StartTime" TO start_time;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER INDEX "IX_Meetups_Topic" RENAME TO ux_meetups_topic;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE refresh_tokens RENAME COLUMN "UserId" TO user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE refresh_tokens RENAME COLUMN "TokenId" TO token_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups_users_signup ADD CONSTRAINT pk_meetups_users_signup PRIMARY KEY (meetup_id, signed_up_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE users ADD CONSTRAINT pk_users PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups ADD CONSTRAINT pk_meetups PRIMARY KEY (id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE refresh_tokens ADD CONSTRAINT pk_refresh_tokens PRIMARY KEY (token_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    CREATE INDEX ix_refresh_tokens_user_id ON refresh_tokens (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups_users_signup ADD CONSTRAINT fk_meetups_users_signup_users_signed_up_user_id FOREIGN KEY (signed_up_user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE meetups_users_signup ADD CONSTRAINT fk_meetups_users_signup_meetups_signed_up_user_id FOREIGN KEY (meetup_id) REFERENCES meetups (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    ALTER TABLE refresh_tokens ADD CONSTRAINT fk_users_refresh_tokens_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220116100522_FollowNamingConventions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220116100522_FollowNamingConventions', '6.0.1');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_users_signup DROP CONSTRAINT fk_meetups_users_signup_meetups_signed_up_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_users_signup DROP CONSTRAINT fk_meetups_users_signup_users_signed_up_user_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_users_signup DROP CONSTRAINT pk_meetups_users_signup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_users_signup RENAME TO meetups_guests_signup;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_guests_signup RENAME COLUMN signed_up_user_id TO signed_up_guest_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER INDEX ix_meetups_users_signup_signed_up_user_id RENAME TO ix_meetups_guests_signup_signed_up_guest_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_guests_signup ADD CONSTRAINT pk_meetups_guests_signup PRIMARY KEY (meetup_id, signed_up_guest_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE users ADD account_type text NOT NULL DEFAULT 'guest';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_guests_signup ADD CONSTRAINT fk_meetups_guests_signup_guests_signed_up_guest_id FOREIGN KEY (signed_up_guest_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
    ALTER TABLE meetups_guests_signup ADD CONSTRAINT fk_meetups_guests_signup_meetups_meetup_id FOREIGN KEY (meetup_id) REFERENCES meetups (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205183337_AddGuests') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205200800_AddOrganizers') THEN
    ALTER TABLE meetups ADD organizer_id uuid NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205200800_AddOrganizers') THEN
    CREATE INDEX "IX_meetups_organizer_id" ON meetups (organizer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205200800_AddOrganizers') THEN
    ALTER TABLE meetups ADD CONSTRAINT fk_meetups_organizers_organizer_id FOREIGN KEY (organizer_id) REFERENCES users (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220205200800_AddOrganizers') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220206172635_UpdateUserDiscriminator') THEN

                UPDATE users
                SET role = 'Guest'
                WHERE role = 'guest';
                UPDATE users
                SET role = 'Organizer'
                WHERE role = 'organizer';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220206172635_UpdateUserDiscriminator') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220207173657_AddMeetupDurationConversion') THEN

                UPDATE meetups
                SET duration = duration_hours * 60 + duration_minutes;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220207173657_AddMeetupDurationConversion') THEN
    ALTER TABLE meetups DROP COLUMN duration_hours;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220207173657_AddMeetupDurationConversion') THEN
    ALTER TABLE meetups DROP COLUMN duration_minutes;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220207173657_AddMeetupDurationConversion') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220207173657_AddMeetupDurationConversion', '6.0.1');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    ALTER TABLE meetups RENAME COLUMN place TO place_address;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    ALTER TABLE meetups ADD place_city_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    CREATE TABLE cities (
        id uuid NOT NULL,
        name character varying(30) NOT NULL,
        CONSTRAINT pk_cities PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN

                INSERT INTO cities(id, name)
                VALUES ('0863abb7-b3fc-4ae8-ab91-81ae9780a14e', 'Oslo');
                UPDATE meetups
                SET place_city_id = '0863abb7-b3fc-4ae8-ab91-81ae9780a14e';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    CREATE INDEX ix_meetups_place_city_id ON meetups (place_city_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    CREATE UNIQUE INDEX ux_cities_name ON cities (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
    ALTER TABLE meetups ADD CONSTRAINT fk_meetups_cities_place_city_id FOREIGN KEY (place_city_id) REFERENCES cities (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222124412_AddCities') THEN
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
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20220222200452_FixMeetupOrganizerIdIndexName') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20220222200452_FixMeetupOrganizerIdIndexName', '6.0.1');
    END IF;
END $EF$;
COMMIT;

