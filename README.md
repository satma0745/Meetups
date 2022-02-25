# Meetups


## How to run


### Clone

Run the following command to clone a repo (by default it will create a new
directory called `Meetups`)
```
git clone https://github.com/satma0745/Meetups.git
```

### Application configuration

Before running an application in Docker, you must first specify
the configuration parameters. At the root of the project is the
`docker-compose.env.sample` file, **don't modify** it, just make a copy of it
named `docker-compose.env` instead. In this copy, you can override any
parameters you want (the main thing is to remember that you did this, and that
the example commands given may not suit you if you changed the value used in
the example).

### Run application & migrate DB

Run the application using the following command:
```
docker-compose --env-file "./docker-compose.env" up --build
```

Wait a bit until application starts. You can verify that the application is
running by navigating to the `http://locahost:5365/api` url (should open the
Swagger documentation page).

**Attention**: on first launch and each time a new migration has been added to the
application (if You are not sure how to determine this - do this each time You
pull the repository), You will need to migrate the database.
In order to do this:
1. Follow the link `http://localhost:5366`.
2. Login using the `PGADMIN_EMAIL` and `DB_PASSWORD` configuration parameters.
3. Expand the `"Servers" > "Meetups server" > "Databases" > "postrges"` tree.
4. Right click on `"postgres"` and select `"CREATE script"`.
5. In the window that opens, paste the contents of the `migrations.sql` file
(located at the root of the project) and press F5.

**Note**: If at step 3 you notice that the `"Servers"` list is empty, then
right-click on it, select `"Create" > "Server ..."`. Specify the name of the
server (`"Name"` field) `"Meetups server"`. On the `"Connection"` tab, write
`"pg"` in the `"Host"` field and `"postgres"` in the `"Username"` field. Click
the `"Save"` button.
