FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ScuttleBot-Remastered/ScuttleBot-Remastered.csproj", "ScuttleBot-Remastered/"]
RUN dotnet restore "ScuttleBot-Remastered/ScuttleBot-Remastered.csproj"
COPY . .
WORKDIR "/src/ScuttleBot-Remastered"
RUN dotnet build "ScuttleBot-Remastered.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScuttleBot-Remastered.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScuttleBot-Remastered.dll"]
