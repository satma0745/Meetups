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