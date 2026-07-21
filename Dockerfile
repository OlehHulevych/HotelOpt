FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY HotelOpt.sln .
COPY HotelOpt.Api/HotelOpt.Api.csproj HotelOpt.Api/
COPY HotelOpt.Application/HotelOpt.Application.csproj HotelOpt.Application/
COPY HotelOpt.Domain/HotelOpt.Domain.csproj HotelOpt.Domain/
COPY HotelOpt.Infrastructure/HotelOpt.Infrastructure.csproj HotelOpt.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish HotelOpt.Api/HotelOpt.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HotelOpt.Api.dll"]
