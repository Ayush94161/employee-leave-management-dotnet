FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY EmployeeLeaveManagement.sln .
COPY src/LeaveManagement.Api/LeaveManagement.Api.csproj src/LeaveManagement.Api/
COPY tests/LeaveManagement.Tests/LeaveManagement.Tests.csproj tests/LeaveManagement.Tests/
RUN dotnet restore
COPY . .
RUN dotnet test -c Release --no-restore
RUN dotnet publish src/LeaveManagement.Api/LeaveManagement.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "LeaveManagement.Api.dll"]
