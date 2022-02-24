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
You need to specify connection parameters for a PostgreSQl:
- `Host`: server that hosts PostgreSQL DBMS
- `Port`: server's port that exposes PostgreSQL connection
- `Database`: name of a database that will used to persist application state
- `Username`: name of the users to perform migration action and connect as
- `Password`: password for the provided username (this is the only parameter that You **forced** to override - for local installation just provide the password You specified in the setup wizard)

Now You can (but not required to) delete all **not overridden** fields.


### Update DB & run application

Now You just need to setup/update a DB (do not forget to come back to the project root directory):
```
dotnet ef database update --project .\Backend\Meetups.Backend.Application.Modules.Persistence --startup-project .\Backend\Meetups.Backend.WebApi
```
**Note**: You will need to repeat this step each time You pull changes from the remote origin, because it can contain DB schema changes.

And run the application:
```
dotnet run --project .\Backend\Meetups.Backend.WebApi
```

Now You can go to the `http://localhost:5265/api` url (note, that Your port may be different, if You changed it in the `launchSettings.json`).
