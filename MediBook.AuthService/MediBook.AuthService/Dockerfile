FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MediBook.AuthService.csproj", "./"]
RUN dotnet restore "MediBook.AuthService.csproj"
COPY . .
RUN dotnet build "MediBook.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediBook.AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MediBook.AuthService.dll"]
