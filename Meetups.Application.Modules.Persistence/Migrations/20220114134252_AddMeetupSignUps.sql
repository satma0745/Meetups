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