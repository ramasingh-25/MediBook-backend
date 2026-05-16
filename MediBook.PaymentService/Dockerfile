FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MediBook.PaymentService.csproj", "./"]
RUN dotnet restore "MediBook.PaymentService.csproj"
COPY . .
RUN dotnet build "MediBook.PaymentService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediBook.PaymentService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MediBook.PaymentService.dll"]
