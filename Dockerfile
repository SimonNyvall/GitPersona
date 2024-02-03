FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY *.fsproj ./
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0

RUN apt-get update && apt-get install -y sqlite3

WORKDIR /app
COPY --from=build-env /app/out .

COPY init.sql .

RUN mkdir Repository
WORKDIR /app/Repository

RUN sqlite3 git-persona.db < ../init.sql
RUN sqlite3 git-persona.db ".tables"

RUN chown root:root /app/Repository/git-persona.db
RUN chmod 664 /app/Repository/git-persona.db

ENV SQLITE_DB_PATH=/app/Repository/mydatabase.db
ENV GIT_CONFIG_PATH="~/.gitconfig"

VOLUME /app/Repository

ENTRYPOINT ["dotnet", "/app/GitPersona.dll"]

# docker run -v ~/.git-config:/app/Repository ...
