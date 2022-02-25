FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Meetups.Domain/*.csproj", "./Meetups.Domain/"]
COPY ["Meetups.Application/*.csproj", "./Meetups.Application/"]
COPY ["Meetups.Application.Modules.Auth/*.csproj", "./Meetups.Application.Modules.Auth/"]
COPY ["Meetups.Application.Modules.Persistence/*.csproj", "./Meetups.Application.Modules.Persistence/"]
COPY ["Meetups.WebApi/*.csproj", "./Meetups.WebApi/"]
COPY ["Meetups.sln", "./"]
RUN dotnet restore "Meetups.sln"
COPY . .

RUN dotnet publish "Meetups.sln" -c Debug -o /publish


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
EXPOSE 80

COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Meetups.WebApi.dll"]