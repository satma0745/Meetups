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