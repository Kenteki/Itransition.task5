
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["ItransitionTask5.sln", "./"]
COPY ["ItransitionTask5.API/ItransitionTask5.API.csproj", "ItransitionTask5.API/"]
COPY ["ItransitionTask5.Application/ItransitionTask5.Application.csproj", "ItransitionTask5.Application/"]
COPY ["ItransitionTask5.Core/ItransitionTask5.Core.csproj", "ItransitionTask5.Core/"]
COPY ["ItransitionTask5.Persistence/ItransitionTask5.Persistence.csproj", "ItransitionTask5.Persistence/"]


RUN dotnet restore "ItransitionTask5.API/ItransitionTask5.API.csproj"
COPY . .
WORKDIR "/src/ItransitionTask5.API"
RUN dotnet build "ItransitionTask5.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ItransitionTask5.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ItransitionTask5.API.dll"]