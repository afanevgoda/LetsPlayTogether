FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LetsPlayTogether.csproj", "./"]
RUN dotnet restore "LetsPlayTogether.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "LetsPlayTogether.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LetsPlayTogether.csproj" -c Release -o /app/publish

FROM node AS node-builder
WORKDIR /node
COPY ./ClientApp /node
RUN npm install
RUN npm run build

FROM base AS final
WORKDIR /app
run mkdir /app/wwwroot
#COPY --from=dotnet-publish /app/publish .
COPY --from=node-builder /node/build ./wwwroot
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LetsPlayTogether.dll"]