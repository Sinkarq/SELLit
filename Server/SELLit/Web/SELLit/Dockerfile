#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Web/SELLit/SELLit.Server.csproj", "Web/SELLit/"]
COPY ["Data/SELLit.Data.Common/SELLit.Data.Common.csproj", "Data/SELLit.Data.Common/"]
COPY ["Data/SELLit.Data.Models/SELLit.Data.Models.csproj", "Data/SELLit.Data.Models/"]
COPY ["SELLit.Common/SELLit.Common.csproj", "SELLit.Common/"]
COPY ["Data/SELLit.Data/SELLit.Data.csproj", "Data/SELLit.Data/"]
RUN dotnet restore "Web/SELLit/SELLit.Server.csproj"
COPY . .
WORKDIR "/src/Web/SELLit"
RUN dotnet build "SELLit.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SELLit.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SELLit.Server.dll"]