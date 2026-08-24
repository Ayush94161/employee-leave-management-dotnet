FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EmployeeLeaveManagement.sln .
COPY src/LeaveManagement.Api/LeaveManagement.Api.csproj src/LeaveManagement.Api/
COPY tests/LeaveManagement.Tests/LeaveManagement.Tests.csproj tests/LeaveManagement.Tests/

RUN dotnet restore

COPY . .

ENV ConnectionStrings__Default="Data Source=/tmp/leave-management-tests.db"
RUN dotnet test -c Release --no-restore
RUN dotnet publish src/LeaveManagement.Api/LeaveManagement.Api.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/* \
    && mkdir -p /app/data \
    && chown -R $APP_UID:$APP_UID /app/data

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

USER $APP_UID

ENTRYPOINT ["dotnet", "LeaveManagement.Api.dll"]