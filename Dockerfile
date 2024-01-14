# Use the ASP.NET Core SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the CSPROJ file and restore any dependencies (via NUGET)
COPY ["ShopFront.WebApi/ShopFront.WebApi.csproj", "ShopFront.WebApi/"]
RUN dotnet restore "ShopFront.WebApi/ShopFront.WebApi.csproj"

# Copy the project files and build the release
COPY . ./
RUN dotnet publish "ShopFront.WebApi/ShopFront.WebApi.csproj" -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN apt update && apt upgrade -y
RUN apt install curl -y

ENV ASPNETCORE_URLS=http://+:5050
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5050
ENTRYPOINT ["dotnet", "ShopFront.WebApi.dll"]