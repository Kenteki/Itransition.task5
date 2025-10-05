FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ItransitionTask5.API/ItransitionTask5.API.csproj ItransitionTask5.API/
COPY ItransitionTask5.Application/*.csproj ItransitionTask5.Application/
COPY ItransitionTask5.Core/*.csproj ItransitionTask5.Core/
COPY ItransitionTask5.Persistence/*.csproj ItransitionTask5.Persistence/

RUN dotnet restore ItransitionTask5.API/ItransitionTask5.API.csproj
COPY . .

WORKDIR /src/ItransitionTask5.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ItransitionTask5.API.dll"]
