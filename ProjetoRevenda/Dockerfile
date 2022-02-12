FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

COPY ./dist .

ENTRYPOINT ["dotnet", "ProjetoRevenda.dll"]