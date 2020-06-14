#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BlazorWebAssemblyOidc/Server/BlazorWebAssemblyOidc.Server.csproj", "BlazorWebAssemblyOidc/Server/"]
COPY ["BlazorWebAssemblyOidc/Shared/BlazorWebAssemblyOidc.Shared.csproj", "BlazorWebAssemblyOidc/Shared/"]
COPY ["BlazorWebAssemblyOidc/Client/BlazorWebAssemblyOidc.Client.csproj", "BlazorWebAssemblyOidc/Client/"]
RUN dotnet restore "BlazorWebAssemblyOidc/Server/BlazorWebAssemblyOidc.Server.csproj"
COPY . .
WORKDIR "/src/BlazorWebAssemblyOidc/Server"
RUN dotnet build "BlazorWebAssemblyOidc.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorWebAssemblyOidc.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
## ENTRYPOINT ["dotnet", "BlazorWebAssemblyOidc.Server.dll"]
CMD dotnet BlazorWebAssemblyOidc.Server.dll
## Heroku deployment ## CMD ASPNETCORE_URLS=http://*:$PORT dotnet BlazorWebAssemblyOidc.Server.dll