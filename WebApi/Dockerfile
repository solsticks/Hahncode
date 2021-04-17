FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore .
COPY . ./
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app
EXPOSE 80
COPY --from=build /app/output .

ENTRYPOINT ["dotnet", "Hahn.ApplicationProcess.December2020.Web.dll"]
