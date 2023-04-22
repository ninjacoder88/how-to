#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HowTo.WebApp/HowTo.WebApp.csproj", "HowTo.WebApp/"]
RUN dotnet restore "HowTo.WebApp/HowTo.WebApp.csproj"
COPY . .
WORKDIR "/src/HowTo.WebApp"
RUN dotnet build "HowTo.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HowTo.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HowTo.WebApp.dll"]