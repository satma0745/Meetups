# Meetups


## How to run


### Dependencies

- PostgreSQL 14
- .Net SDK 6


### Clone and restore

Run the following command to clone a repo (by default it will create a new directory called `Meetups`)
```
git clone https://github.com/satma0745/Meetups.git
```

Go to the `Meetups` and run
```
dotnet restore
dotnet tool restore
```


### Application configuration

Go to the `Backend` > `Meetups.Backend.WebApi` > `Properties` folder.
Here You will see 2 files: the `launchSettings.json` and the `appSettings.json`.

As for `launchSettings.json`, there are only one property that can be interesting for consumers: `applicationUrl`.
You can change the port number, just remember to follow this pattern: `http://localhost:<port>`.

As for `appSettings.json`, it an application settings template file.
You **should not modify** it, but instead You can copy it and rename to a `appSettings.Development.json`.
Now You can modify this copy and it will not interrupt when You try to pull recent changes from an origin.
You need to specify connection string for a PostgreSQl; the format is already here, You just need to fill all placeholders:
- `{host}`: change it to `localhost`
- `{port}`: the default port for local PostgreSQL installation is `5432`
- `{database}`: this is the name of a DB that will be created later, so I personally recomment You using the same name as the project root folder name (`meetups`)
- `{username}`: You can use the default username `postgres`, or create a new user with the `CREATEDB` privilege manually (the choice is up to You)
- `{password}`: place here the password that You specified during PostgreSQL installation (or for the manually created user)
Now You can (but not required to) delete all duplicating fields
(if You didn't change anything but PostgreSQL connection string, then You can delete the whole `Auth` section).


### Update DB & run application

Now You just need to setup/update a DB (do not forget to come back to the project root directory):
```
dotnet ef database update --project .\Backend\Meetups.Backend.Persistence --startup-project .\Backend\Meetups.Backend.WebApi
```
**Note**: You will need to repeate this step each time You pull changes from remote origin, because it can contain DB schema changes.

And run the application:
```
dotnet run --project .\Backend\Meetups.Backend.WebApi
```

Now You can go to the `http://localhost:5265/api` url (note, that Your port may be different, if You changed it in the `launchSettings.json`).
