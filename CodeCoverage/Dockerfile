FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-arm64v8 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["CodeCoverage/CodeCoverage.csproj", "CodeCoverage/"]
RUN dotnet restore "CodeCoverage/CodeCoverage.csproj"
COPY . .
WORKDIR "/CodeCoverage"
RUN dotnet build "CodeCoverage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeCoverage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeCoverage.dll"]