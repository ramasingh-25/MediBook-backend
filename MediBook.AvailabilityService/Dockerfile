FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MediBook.AvailabilityService.csproj", "./"]
RUN dotnet restore "MediBook.AvailabilityService.csproj"
COPY . .
RUN dotnet build "MediBook.AvailabilityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediBook.AvailabilityService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MediBook.AvailabilityService.dll"]
