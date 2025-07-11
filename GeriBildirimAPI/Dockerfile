FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GeriBildirimAPI.csproj", "./"]
RUN dotnet restore "GeriBildirimAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "GeriBildirimAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GeriBildirimAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GeriBildirimAPI.dll"]
