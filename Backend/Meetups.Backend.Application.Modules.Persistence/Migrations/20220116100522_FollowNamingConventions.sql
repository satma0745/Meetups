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