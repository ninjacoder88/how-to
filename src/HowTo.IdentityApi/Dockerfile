#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /code
COPY . .
RUN dotnet restore "HowTo.IdentityApi.csproj"
RUN dotnet build "HowTo.IdentityApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HowTo.IdentityApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HowTo.IdentityApi.dll"]