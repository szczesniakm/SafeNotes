FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SafeNotes.Api/SafeNotes.Api.csproj", "SafeNotes.Api/"]
COPY ["SafeNotes.Infrastructure/SafeNotes.Infrastructure.csproj", "SafeNotes.Infrastructure/"]
COPY ["SafeNotes.Domain/SafeNotes.Domain.csproj", "SafeNotes.Domain/"]
COPY ["SafeNotes.Application/SafeNotes.Application.csproj", "SafeNotes.Application/"]
RUN dotnet restore "SafeNotes.Api/SafeNotes.Api.csproj"
COPY . .
WORKDIR "/src/SafeNotes.Api"
RUN dotnet build "SafeNotes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SafeNotes.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SafeNotes.Api.dll", "--urls", "http://0.0.0.0:5000"]