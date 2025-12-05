FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /

COPY WebApi/WebApi.csproj WebApi/
COPY Migrations/Migrations.csproj Migrations/
COPY Models/Models.csproj Models/
COPY Project.Common/Project.Common.csproj Project.Common/
COPY Project.Messages/Project.Messages.csproj Project.Messages/

RUN dotnet restore "WebApi/WebApi.csproj"

COPY WebApi/ WebApi/
COPY Migrations/ Migrations/
COPY Models/ Models/
COPY Project.Common/ Project.Common/
COPY Project.Messages/ Project.Messages/

RUN dotnet publish "WebApi/WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]