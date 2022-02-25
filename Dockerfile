FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["Contracts/Meetup.Contract/*.csproj", "./Contracts/Meetup.Contract/"]
COPY ["Backend/Meetups.Backend.Domain/*.csproj", "./Backend/Meetups.Backend.Domain/"]
COPY ["Backend/Meetups.Backend.Application/*.csproj", "./Backend/Meetups.Backend.Application/"]
COPY ["Backend/Meetups.Backend.Application.Modules.Auth/*.csproj", "./Backend/Meetups.Backend.Application.Modules.Auth/"]
COPY ["Backend/Meetups.Backend.Application.Modules.Persistence/*.csproj", "./Backend/Meetups.Backend.Application.Modules.Persistence/"]
COPY ["Backend/Meetups.Backend.WebApi/*.csproj", "./Backend/Meetups.Backend.WebApi/"]
COPY ["Meetups.sln", "./"]
RUN dotnet restore "Meetups.sln"
COPY . .

RUN dotnet publish "Meetups.sln" -c Debug -o /publish


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
EXPOSE 80

COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Meetups.Backend.WebApi.dll"]