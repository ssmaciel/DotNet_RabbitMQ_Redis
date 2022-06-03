#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Job1/Job1.csproj", "Job1/"]
RUN dotnet restore "Job1/Job1.csproj"
COPY . .
WORKDIR "/src/Job1"
RUN dotnet build "Job1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Job1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Job1.dll"]
