FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /
COPY ["src/Admin/Server/YourBrand.Server.csproj", "src/Admin/Server/"]
COPY ["NuGet.config", "/"]
RUN dotnet restore "src/Admin/Server/YourBrand.Server.csproj"
COPY . .
RUN dotnet test "/src/Admin/Server.UnitTests"
WORKDIR "/src/Admin/Server"
RUN dotnet build "YourBrand.Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "YourBrand.Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBrand.Server.dll"]
