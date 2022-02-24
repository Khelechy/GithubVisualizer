#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GithubVisualizer/GithubVisualizer.csproj", "GithubVisualizer/"]
RUN dotnet restore "GithubVisualizer/GithubVisualizer.csproj"
COPY . .
WORKDIR "/src/GithubVisualizer"
RUN dotnet build "GithubVisualizer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GithubVisualizer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet GithubVisualizer.dll
#ENTRYPOINT ["dotnet", "GithubVisualizer.dll"]